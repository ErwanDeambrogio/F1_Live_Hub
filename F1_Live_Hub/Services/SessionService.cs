using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using F1_Live_Hub.Models;

namespace F1_Live_Hub.Services
{
    public class Championship
    {
        public string championshipId { get; set; }
        public string championshipName { get; set; }
        public string url { get; set; }
        public int year { get; set; }
    }

    public class Circuit
    {
        public string circuitId { get; set; }
        public string circuitName { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string circuitLength { get; set; }
        public string lapRecord { get; set; }
        public int firstParticipationYear { get; set; }
        public int corners { get; set; }
        public string fastestLapDriverId { get; set; }
        public string fastestLapTeamId { get; set; }
        public int? fastestLapYear { get; set; }
        public string url { get; set; }
    }

    public class FastLap
    {
        public string fast_lap { get; set; }
        public string fast_lap_driver_id { get; set; }
        public string fast_lap_team_id { get; set; }
    }

    public class Fp1 { public string date { get; set; } public string time { get; set; } }
    public class Fp2 { public string date { get; set; } public string time { get; set; } }
    public class Fp3 { public string date { get; set; } public string time { get; set; } }
    public class Qualy { public string date { get; set; } public string time { get; set; } }
    public class RaceSchedule { public string date { get; set; } public string time { get; set; } }
    public class SprintQualy { public string date { get; set; } public string time { get; set; } }
    public class SprintRace { public string date { get; set; } public string time { get; set; } }

    public class Schedule
    {
        public RaceSchedule race { get; set; }
        public Qualy qualy { get; set; }
        public Fp1 fp1 { get; set; }
        public Fp2 fp2 { get; set; }
        public Fp3 fp3 { get; set; }
        public SprintQualy sprintQualy { get; set; }
        public SprintRace sprintRace { get; set; }
    }

    public class Race
    {
        public string raceId { get; set; }
        public string championshipId { get; set; }
        public string raceName { get; set; }
        public Schedule schedule { get; set; }
        public int? laps { get; set; }
        public int round { get; set; }
        public string url { get; set; }
        public FastLap fast_lap { get; set; }
        public Circuit circuit { get; set; }
        public Winner winner { get; set; }
        public TeamWinner teamWinner { get; set; }
    }

    public class Root
    {
        public string api { get; set; }
        public string url { get; set; }
        public int total { get; set; }
        public int season { get; set; }
        public Championship championship { get; set; }
        public List<Race> race { get; set; }
    }

    public class TeamWinner
    {
        public string teamId { get; set; }
        public string teamName { get; set; }
        public string country { get; set; }
        public int firstAppearance { get; set; }
        public int constructorsChampionships { get; set; }
        public int driversChampionships { get; set; }
        public string url { get; set; }
    }

    public class Winner
    {
        public string driverId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string country { get; set; }
        public string birthday { get; set; }
        public int number { get; set; }
        public string shortName { get; set; }
        public string url { get; set; }
    }

    public class SessionService
    {
        private readonly HttpClient _httpClient;

        public SessionService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Course> GetNextRaceAsync()
            => await FetchCourse("https://f1api.dev/api/current/next");

        public async Task<Course> GetLastRaceAsync()
            => await FetchCourseWithResults("https://f1api.dev/api/current/last");

        public async Task<Course> GetRaceByYearRoundAsync(int year, int round)
            => await FetchCourseWithResults($"https://f1api.dev/api/{year}/{round}");

        private async Task<Course> FetchCourse(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            var root = JsonConvert.DeserializeObject<Root>(response);
            if (root?.race == null || root.race.Count == 0) return null;
            return BuildCourse(root.race[0], root.season);
        }

        private async Task<Course> FetchCourseWithResults(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            var root = JsonConvert.DeserializeObject<Root>(response);
            if (root?.race == null || root.race.Count == 0) return null;

            var race = root.race[0];
            var course = BuildCourse(race, root.season);

            if (string.IsNullOrEmpty(race.winner?.name))
            {
                try
                {
                    string resultsUrl = $"https://f1api.dev/api/{root.season}/{race.round}/race/results";
                    var resResponse = await _httpClient.GetStringAsync(resultsUrl);
                    var resJson = JObject.Parse(resResponse);
                    var results = resJson["raceResults"] as JArray;
                    if (results != null)
                    {
                        foreach (var r in results)
                        {
                            if (r["position"]?.ToObject<int>() == 1)
                            {
                                course.WinnerName = r["driver"]?["name"]?.ToString() ?? "—";
                                course.WinnerSurname = r["driver"]?["surname"]?.ToString() ?? "—";
                                course.WinnerShortName = r["driver"]?["shortName"]?.ToString() ?? "—";
                                course.WinnerCountry = r["driver"]?["country"]?.ToString() ?? "—";
                                course.WinnerNumber = r["driver"]?["number"]?.ToObject<int>() ?? 0;
                                course.TeamWinnerName = r["team"]?["teamName"]?.ToString() ?? "—";
                                course.TeamWinnerCountry = r["team"]?["country"]?.ToString() ?? "—";
                                break;
                            }
                        }
                        foreach (var r in results)
                        {
                            bool hasFl = r["fastestLap"]?.ToObject<bool>() ?? false;
                            if (hasFl)
                            {
                                course.FastLap = r["fastestLapTime"]?.ToString() ?? "—";
                                course.FastLapDriverId = r["driver"]?["shortName"]?.ToString() ?? "—";
                                break;
                            }
                        }
                    }
                }
                catch { }
            }

            return course;
        }

        private Course BuildCourse(Race race, int season)
        {
            return new Course
            {
                RaceId = race.raceId,
                ChampionshipId = race.championshipId,
                RaceName = race.raceName ?? "—",
                Round = race.round,
                Laps = race.laps,
                Url = race.url,
                Year = season.ToString(),

                CircuitName = race.circuit?.circuitName ?? "—",
                Country = race.circuit?.country ?? "—",
                City = race.circuit?.city ?? "—",
                CircuitLength = race.circuit?.circuitLength ?? "—",
                LapRecord = race.circuit?.lapRecord ?? "—",
                Corners = race.circuit?.corners ?? 0,
                CircuitFirstYear = race.circuit?.firstParticipationYear ?? 0,

                RaceDate = race.schedule?.race?.date ?? "—",
                RaceTime = race.schedule?.race?.time ?? "—",
                QualyDate = race.schedule?.qualy?.date ?? "—",
                QualyTime = race.schedule?.qualy?.time ?? "—",
                Fp1Date = race.schedule?.fp1?.date ?? "—",
                Fp1Time = race.schedule?.fp1?.time ?? "—",
                Fp2Date = race.schedule?.fp2?.date ?? "—",
                Fp2Time = race.schedule?.fp2?.time ?? "—",
                Fp3Date = race.schedule?.fp3?.date ?? "—",
                Fp3Time = race.schedule?.fp3?.time ?? "—",
                SprintRaceDate = race.schedule?.sprintRace?.date ?? "—",
                SprintRaceTime = race.schedule?.sprintRace?.time ?? "—",
                SprintQualyDate = race.schedule?.sprintQualy?.date ?? "—",

                WinnerName = race.winner?.name ?? "—",
                WinnerSurname = race.winner?.surname ?? "—",
                WinnerShortName = race.winner?.shortName ?? "—",
                WinnerCountry = race.winner?.country ?? "—",
                WinnerNumber = race.winner?.number ?? 0,
                TeamWinnerName = race.teamWinner?.teamName ?? "—",
                TeamWinnerCountry = race.teamWinner?.country ?? "—",

                FastLap = race.fast_lap?.fast_lap ?? "—",
                FastLapDriverId = race.fast_lap?.fast_lap_driver_id ?? "—",
                FastLapTeamId = race.fast_lap?.fast_lap_team_id ?? "—",
            };
        }
    }
}