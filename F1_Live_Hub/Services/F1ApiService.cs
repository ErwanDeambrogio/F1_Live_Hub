using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using F1_Live_Hub.Models;

namespace F1_Live_Hub.Services
{
    public class DriverApi
    {
        public string driverId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string nationality { get; set; }
        public string birthday { get; set; }
        public int? number { get; set; }
        public string shortName { get; set; }
        public string url { get; set; }
        public string teamId { get; set; }
    }

    public class DriversRoot
    {
        public int season { get; set; }
        public string championshipId { get; set; }
        public List<DriverApi> drivers { get; set; }
    }

    public class F1ApiService
    {
        private readonly HttpClient _httpClient;

        public F1ApiService()
        {
            _httpClient = new HttpClient();
        }

        // ── Helper : lit un int nullable sans crash ───────────────
        private static int SafeInt(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null) return 0;
            if (token.Type == JTokenType.Integer) return token.ToObject<int>();
            int.TryParse(token.ToString(), out int v);
            return v;
        }

        private static double SafeDouble(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null) return 0;
            if (token.Type == JTokenType.Float ||
                token.Type == JTokenType.Integer) return token.ToObject<double>();
            double.TryParse(token.ToString(), out double v);
            return v;
        }

        private static string SafeStr(JToken token, string fallback = "—")
        {
            if (token == null || token.Type == JTokenType.Null) return fallback;
            return token.ToString();
        }

        private static bool SafeBool(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null) return false;
            if (token.Type == JTokenType.Boolean) return token.ToObject<bool>();
            return false;
        }

        // ── Tous les pilotes saison courante ──────────────────────
        public async Task<List<Pilote>> GetCurrentDriversAsync()
        {
            var response = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/drivers");
            var root = JsonConvert.DeserializeObject<DriversRoot>(response);

            var pilotes = new List<Pilote>();
            if (root?.drivers == null) return pilotes;

            foreach (var d in root.drivers)
            {
                pilotes.Add(new Pilote
                {
                    DriverId = d.driverId ?? "",
                    TeamId = d.teamId ?? "",
                    Name = d.name ?? "—",
                    Surname = d.surname ?? "—",
                    Nationality = d.nationality ?? "—",
                    Birthday = d.birthday ?? "—",
                    Number = d.number ?? 0,
                    ShortName = d.shortName ?? "—",
                    Url = d.url ?? "",
                });
            }
            return pilotes;
        }

        // ── Détail complet pilote ─────────────────────────────────
        public async Task<Pilote> GetDriverDetailAsync(string driverId)
        {
            // 1. Infos pilote + résultats courses
            var responseCurrent = await _httpClient.GetStringAsync(
                $"https://f1api.dev/api/current/drivers/{driverId}?limit=30");

            var jsonCurrent = JObject.Parse(responseCurrent);
            var d = jsonCurrent["driver"];
            var team = jsonCurrent["team"];

            var pilote = new Pilote
            {
                DriverId = SafeStr(d?["driverId"], driverId),
                Name = SafeStr(d?["name"]),
                Surname = SafeStr(d?["surname"]),
                Nationality = SafeStr(d?["nationality"]),
                Birthday = SafeStr(d?["birthday"]),
                Number = SafeInt(d?["number"]),
                ShortName = SafeStr(d?["shortName"]),
                Url = SafeStr(d?["url"], ""),
                TeamId = SafeStr(team?["teamId"], ""),
                TeamName = SafeStr(team?["teamName"]),
                TeamNationality = SafeStr(team?["teamNationality"]),
                TeamUrl = SafeStr(team?["url"], ""),
                ConstructorsChampionships = SafeInt(team?["constructorsChampionships"]),
                DriversChampionships = SafeInt(team?["driversChampionships"]),
            };

            BuildStats(pilote, jsonCurrent["results"] as JArray);

            // 2. Classement pilotes
            var responseDriversChamp = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/drivers-championship?limit=30");

            var champJson = JObject.Parse(responseDriversChamp);
            var champArray = champJson["drivers_championship"] as JArray;

            if (champArray != null)
            {
                foreach (var item in champArray)
                {
                    if (SafeStr(item["driverId"], "") != driverId) continue;

                    pilote.ChampionshipPosition = SafeInt(item["position"]);
                    pilote.ChampionshipPoints = SafeDouble(item["points"]);
                    pilote.ChampionshipWins = SafeInt(item["wins"]);

                    var teamNode = item["team"];
                    if (teamNode != null)
                    {
                        if (pilote.TeamName == "—")
                            pilote.TeamName = SafeStr(teamNode["teamName"]);
                        if (pilote.TeamNationality == "—")
                            pilote.TeamNationality = SafeStr(teamNode["country"]);
                        if (pilote.ConstructorsChampionships == 0)
                            pilote.ConstructorsChampionships =
                                SafeInt(teamNode["constructorsChampionships"]);
                        if (pilote.DriversChampionships == 0)
                            pilote.DriversChampionships =
                                SafeInt(teamNode["driversChampionships"]);
                    }
                    break;
                }
            }

            // 3. Classement constructeurs
            var responseConstructors = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/constructors-championship?limit=30");

            var constrJson = JObject.Parse(responseConstructors);
            var constrArray = constrJson["constructors_championship"] as JArray;

            if (constrArray != null && !string.IsNullOrEmpty(pilote.TeamId))
            {
                foreach (var item in constrArray)
                {
                    if (SafeStr(item["teamId"], "") != pilote.TeamId) continue;

                    pilote.ConstructorPosition = SafeInt(item["position"]);
                    pilote.ConstructorPoints = SafeDouble(item["points"]);
                    pilote.ConstructorWins = SafeInt(item["wins"]);

                    var teamNode = item["team"];
                    if (teamNode != null)
                    {
                        if (pilote.TeamNationality == "—")
                            pilote.TeamNationality = SafeStr(teamNode["country"]);
                        if (pilote.ConstructorsChampionships == 0)
                            pilote.ConstructorsChampionships =
                                SafeInt(teamNode["constructorsChampionships"]);
                    }
                    break;
                }
            }

            return pilote;
        }

        // ── Détail par année spécifique ───────────────────────────
        public async Task<Pilote> GetDriverDetailByYearAsync(string driverId, int year)
        {
            var response = await _httpClient.GetStringAsync(
                $"https://f1api.dev/api/{year}/drivers/{driverId}?limit=30");

            var json = JObject.Parse(response);
            var d = json["driver"];
            var team = json["team"];

            var pilote = new Pilote
            {
                DriverId = SafeStr(d?["driverId"], ""),
                Name = SafeStr(d?["name"]),
                Surname = SafeStr(d?["surname"]),
                Nationality = SafeStr(d?["nationality"]),
                Birthday = SafeStr(d?["birthday"]),
                Number = SafeInt(d?["number"]),
                ShortName = SafeStr(d?["shortName"]),
                Url = SafeStr(d?["url"], ""),
                TeamId = SafeStr(team?["teamId"], ""),
                TeamName = SafeStr(team?["teamName"]),
                TeamNationality = SafeStr(team?["teamNationality"]),
                TeamUrl = SafeStr(team?["url"], ""),
                ConstructorsChampionships = SafeInt(team?["constructorsChampionships"]),
                DriversChampionships = SafeInt(team?["driversChampionships"]),
            };

            BuildStats(pilote, json["results"] as JArray);
            return pilote;
        }

        // ── Recherche par nom ─────────────────────────────────────
        public async Task<List<Pilote>> SearchDriversAsync(string query)
        {
            var response = await _httpClient.GetStringAsync(
                $"https://f1api.dev/api/drivers/search?q={query}");
            var root = JsonConvert.DeserializeObject<DriversRoot>(response);

            var pilotes = new List<Pilote>();
            if (root?.drivers == null) return pilotes;

            foreach (var d in root.drivers)
            {
                pilotes.Add(new Pilote
                {
                    DriverId = d.driverId ?? "",
                    TeamId = d.teamId ?? "",
                    Name = d.name ?? "—",
                    Surname = d.surname ?? "—",
                    Nationality = d.nationality ?? "—",
                    Birthday = d.birthday ?? "—",
                    Number = d.number ?? 0,
                    ShortName = d.shortName ?? "—",
                    Url = d.url ?? "",
                });
            }
            return pilotes;
        }

        // ── Construction stats depuis résultats ───────────────────
        private void BuildStats(Pilote pilote, JArray resultsArray)
        {
            var raceResults = new List<PiloteRaceResult>();
            int wins = 0, podiums = 0, retirements = 0;
            double totalPoints = 0;
            int bestFinish = 99;

            if (resultsArray != null)
            {
                foreach (var item in resultsArray)
                {
                    var race = item["race"];
                    var result = item["result"];
                    var sprint = item["sprintResult"];

                    if (result == null) continue;

                    // Position — peut être int ou string "NC"
                    var posToken = result["finishingPosition"];
                    int pos = 99;
                    if (posToken != null && posToken.Type == JTokenType.Integer)
                        pos = posToken.ToObject<int>();
                    else if (posToken != null && posToken.Type != JTokenType.Null)
                        int.TryParse(posToken.ToString(), out pos);

                    double pts = SafeDouble(result["pointsObtained"]);
                    int gridPos = SafeInt(result["gridPosition"]);
                    bool retired = SafeBool(result["retired"]);
                    string time = SafeStr(result["raceTime"], "—");

                    double sprintPts = 0;
                    int sprintPos = 0;
                    if (sprint != null && sprint.Type != JTokenType.Null)
                    {
                        sprintPts = SafeDouble(sprint["pointsObtained"]);
                        var spPosToken = sprint["finishingPosition"];
                        if (spPosToken != null &&
                            spPosToken.Type == JTokenType.Integer)
                            sprintPos = spPosToken.ToObject<int>();
                    }

                    double totalPtsRace = pts + sprintPts;
                    bool isDnf = retired || time.Contains("DNF");

                    if (pos == 1) wins++;
                    if (pos <= 3 && pos > 0) podiums++;
                    if (isDnf) retirements++;
                    totalPoints += totalPtsRace;
                    if (pos < bestFinish && pos > 0) bestFinish = pos;

                    raceResults.Add(new PiloteRaceResult
                    {
                        RaceName = SafeStr(race?["name"]),
                        RaceDate = SafeStr(race?["date"]),
                        Round = SafeInt(race?["round"]),
                        CircuitName = SafeStr(race?["circuit"]?["name"]),
                        Country = SafeStr(race?["circuit"]?["country"]),
                        FinishingPosition = pos,
                        PositionLabel = pos == 99 ? "NC" : "P" + pos,
                        GridPosition = gridPos,
                        RaceTime = time,
                        PointsObtained = totalPtsRace,
                        Retired = isDnf,
                        SprintPosition = sprintPos,
                        SprintPoints = sprintPts,
                    });
                }
            }

            pilote.Wins = wins;
            pilote.Podiums = podiums;
            pilote.Retirements = retirements;
            pilote.TotalPoints = totalPoints;
            pilote.RacesEntered = raceResults.Count;
            pilote.BestFinish = bestFinish == 99 ? 0 : bestFinish;
            pilote.RaceResults = raceResults;
        }
    }
}