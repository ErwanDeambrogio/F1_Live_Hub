using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using F1_Live_Hub.Models;
using F1_Live_Hub.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace F1_Live_Hub.Views
{
    public partial class ProfilPage : Window
    {
        private readonly F1ApiService _apiService;
        private readonly FavoritesService _favService;
        private readonly AuthService _authService;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _statsFile;
        private DateTime _sessionStart;
        private TimeSpan _totalTimeSpent;

        public ProfilPage()
        {
            InitializeComponent();
            _apiService = new F1ApiService();
            _favService = new FavoritesService();
            _authService = new AuthService();
            _statsFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "F1LiveHub", "stats.json");
            _sessionStart = DateTime.Now;
            LoadStatsAsync();
        }

        // ── Chargement données ────────────────────────────────────
        private async void LoadStatsAsync()
        {
            try
            {
                // Nom utilisateur
                var currentUser = _authService.GetCurrentUser();
                if (!string.IsNullOrEmpty(currentUser))
                    TxtUsername.Text = currentUser;

                // Temps total
                _totalTimeSpent = LoadTotalTime();
                UpdateTimeDisplay();

                // Favoris pilotes
                var favIds = _favService.GetAll();
                var allPilotes = await _apiService.GetCurrentDriversAsync();
                var favPilotes = new List<Pilote>();
                foreach (var id in favIds)
                {
                    var match = allPilotes.Find(d => d.DriverId == id);
                    if (match != null) favPilotes.Add(match);
                }
                FavDriversList.ItemsSource = favPilotes.Count > 0 ? favPilotes : null;
                TxtNoFavDrivers.Visibility = favPilotes.Count == 0
                    ? Visibility.Visible : Visibility.Collapsed;

                await LoadTop3Drivers();
                await LoadTop3Constructors();
                await LoadGrandPrixStats();
            }
            catch (Exception ex)
            {
                TxtError.Text = "❌ " + ex.Message;
                TxtError.Visibility = Visibility.Visible;
            }
        }

        private async System.Threading.Tasks.Task LoadTop3Drivers()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    "https://f1api.dev/api/current/drivers-championship?limit=30");
                var json = JObject.Parse(response);
                var array = json["drivers_championship"] as JArray;
                if (array == null) return;

                var top3 = new List<DriverStandingItem>();
                foreach (var item in array)
                {
                    int pos = item["position"]?.ToObject<int>() ?? 99;
                    if (pos > 3) continue;
                    top3.Add(new DriverStandingItem
                    {
                        Position = pos,
                        Name = item["driver"]?["name"]?.ToString() ?? "—",
                        Surname = item["driver"]?["surname"]?.ToString() ?? "—",
                        ShortName = item["driver"]?["shortName"]?.ToString() ?? "—",
                        TeamId = item["teamId"]?.ToString() ?? "",
                        Points = item["points"]?.ToObject<double>() ?? 0,
                        Wins = item["wins"]?.ToObject<int>() ?? 0,
                    });
                }
                top3.Sort((a, b) => a.Position.CompareTo(b.Position));
                Top3DriversList.ItemsSource = top3;
            }
            catch { }
        }

        private async System.Threading.Tasks.Task LoadTop3Constructors()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    "https://f1api.dev/api/current/constructors-championship?limit=30");
                var json = JObject.Parse(response);
                var array = json["constructors_championship"] as JArray;
                if (array == null) return;

                var top3 = new List<ConstructorStandingItem>();
                foreach (var item in array)
                {
                    int pos = item["position"]?.ToObject<int>() ?? 99;
                    if (pos > 3) continue;
                    top3.Add(new ConstructorStandingItem
                    {
                        Position = pos,
                        TeamId = item["teamId"]?.ToString() ?? "",
                        TeamName = item["team"]?["teamName"]?.ToString() ?? "—",
                        Points = item["points"]?.ToObject<double>() ?? 0,
                        Wins = item["wins"]?.ToObject<int>() ?? 0,
                    });
                }
                top3.Sort((a, b) => a.Position.CompareTo(b.Position));
                Top3ConstructorsList.ItemsSource = top3;
            }
            catch { }
        }

        private async System.Threading.Tasks.Task LoadGrandPrixStats()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    "https://f1api.dev/api/current/last");
                var json = JObject.Parse(response);
                var races = json["race"] as JArray;
                if (races == null || races.Count == 0) return;

                int round = races[0]["round"]?.ToObject<int>() ?? 0;
                int season = json["season"]?.ToObject<int>() ?? DateTime.Now.Year;

                TxtGPPassed.Text = round.ToString();
                TxtCurrentSeason.Text = season.ToString();

                var responseNext = await _httpClient.GetStringAsync(
                    "https://f1api.dev/api/current/next");
                var jsonNext = JObject.Parse(responseNext);
                var racesNext = jsonNext["race"] as JArray;
                if (racesNext != null && racesNext.Count > 0)
                {
                    var next = racesNext[0];
                    TxtNextRace.Text = next["raceName"]?.ToString() ?? "—";
                    TxtNextRound.Text = "Round " + (next["round"]?.ToString() ?? "—");

                    string dateStr = next["schedule"]?["race"]?["date"]?.ToString() ?? "";
                    if (DateTime.TryParse(dateStr, out DateTime raceDate))
                    {
                        int daysLeft = (int)(raceDate - DateTime.Now).TotalDays;
                        TxtDaysLeft.Text = daysLeft > 0
                            ? string.Format("Dans {0} jours", daysLeft)
                            : "Ce week-end !";
                    }
                }
            }
            catch { }
        }

        // ── Gestion temps ─────────────────────────────────────────
        private TimeSpan LoadTotalTime()
        {
            try
            {
                if (!File.Exists(_statsFile)) return TimeSpan.Zero;
                var json = JObject.Parse(File.ReadAllText(_statsFile));
                double mins = json["totalMinutes"]?.ToObject<double>() ?? 0;
                return TimeSpan.FromMinutes(mins);
            }
            catch { return TimeSpan.Zero; }
        }

        private void SaveTotalTime()
        {
            try
            {
                var elapsed = DateTime.Now - _sessionStart;
                var total = _totalTimeSpent + elapsed;
                Directory.CreateDirectory(Path.GetDirectoryName(_statsFile));
                File.WriteAllText(_statsFile,
                    JsonConvert.SerializeObject(new { totalMinutes = total.TotalMinutes }));
            }
            catch { }
        }

        private void UpdateTimeDisplay()
        {
            var session = DateTime.Now - _sessionStart;
            var total = _totalTimeSpent + session;
            TxtTimeTotal.Text = string.Format("{0}h {1:D2}m",
                (int)total.TotalHours, total.Minutes);
            TxtTimeSession.Text = string.Format("{0}m {1:D2}s",
                (int)session.TotalMinutes, session.Seconds);
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveTotalTime();
            base.OnClosed(e);
        }

        // ── Déconnexion ───────────────────────────────────────────
        private void BtnLogout_Click(object sender, MouseButtonEventArgs e)
        {
            SaveTotalTime();
            _authService.Logout();
            var login = new LoginPage();
            login.Show();
            this.Close();
        }

        // ── Navigation ────────────────────────────────────────────
        private void OpenWindow(Window window) { window.Show(); this.Close(); }

        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Tab_Meteo_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CircuitsPage());
        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Saison_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Nav_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
    }

    // ── Modèles locaux ────────────────────────────────────────────
    public class DriverStandingItem
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ShortName { get; set; }
        public string TeamId { get; set; }
        public double Points { get; set; }
        public int Wins { get; set; }

        public string PositionColor
        {
            get
            {
                switch (Position)
                {
                    case 1: return "#FFD700";
                    case 2: return "#C0C0C0";
                    case 3: return "#CD7F32";
                    default: return "#888888";
                }
            }
        }

        public string TeamColor
        {
            get
            {
                switch (TeamId?.ToLower())
                {
                    case "red_bull": return "#3671C6";
                    case "ferrari": return "#E8002D";
                    case "mercedes": return "#27F4D2";
                    case "mclaren": return "#FF8000";
                    case "aston_martin": return "#358C75";
                    case "alpine": return "#FF87BC";
                    case "williams": return "#64C4FF";
                    case "haas": return "#B6BABD";
                    case "kick_sauber": return "#52E252";
                    case "rb": return "#6692FF";
                    default: return "#555555";
                }
            }
        }
    }

    public class ConstructorStandingItem
    {
        public int Position { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public double Points { get; set; }
        public int Wins { get; set; }

        public string PositionColor
        {
            get
            {
                switch (Position)
                {
                    case 1: return "#FFD700";
                    case 2: return "#C0C0C0";
                    case 3: return "#CD7F32";
                    default: return "#888888";
                }
            }
        }

        public string TeamColor
        {
            get
            {
                switch (TeamId?.ToLower())
                {
                    case "red_bull": return "#3671C6";
                    case "ferrari": return "#E8002D";
                    case "mercedes": return "#27F4D2";
                    case "mclaren": return "#FF8000";
                    case "aston_martin": return "#358C75";
                    case "alpine": return "#FF87BC";
                    case "williams": return "#64C4FF";
                    case "haas": return "#B6BABD";
                    case "kick_sauber": return "#52E252";
                    case "rb": return "#6692FF";
                    default: return "#555555";
                }
            }
        }

        public string TeamLogoUrl
        {
            get
            {
                switch (TeamId?.ToLower())
                {
                    case "red_bull":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/9/97/Red_Bull_Racing_logo.svg/200px-Red_Bull_Racing_logo.svg.png";
                    case "ferrari":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/d/d4/Scuderia_Ferrari_Logo.svg/200px-Scuderia_Ferrari_Logo.svg.png";
                    case "mercedes":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/f/fb/Mercedes-Benz_in_Formula_One_logo.svg/200px-Mercedes-Benz_in_Formula_One_logo.svg.png";
                    case "mclaren":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/6/66/McLaren_Racing_logo.svg/200px-McLaren_Racing_logo.svg.png";
                    case "aston_martin":
                        return "https://upload.wikimedia.org/wikipedia/en/thumb/9/9f/Aston_Martin_F1_Logo.svg/200px-Aston_Martin_F1_Logo.svg.png";
                    case "alpine":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/Alpine_F1_Team_Logo.svg/200px-Alpine_F1_Team_Logo.svg.png";
                    case "williams":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/8/80/Williams_Racing_logo_2020.svg/200px-Williams_Racing_logo_2020.svg.png";
                    case "haas":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Haas_F1_Team_Logo.svg/200px-Haas_F1_Team_Logo.svg.png";
                    case "kick_sauber":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Sauber_Motorsport_AG_logo.svg/200px-Sauber_Motorsport_AG_logo.svg.png";
                    case "rb":
                        return "https://upload.wikimedia.org/wikipedia/commons/thumb/8/82/Visa_Cash_App_RB_Formula_One_Team_logo.svg/200px-Visa_Cash_App_RB_Formula_One_Team_logo.svg.png";
                    default: return "";
                }
            }
        }
    }
}