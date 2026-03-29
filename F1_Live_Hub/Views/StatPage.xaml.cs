using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

namespace F1_Live_Hub.Views
{
    public partial class StatPage : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public StatPage()
        {
            InitializeComponent();
            LoadStatsAsync();
        }

        private void OpenWindow(Window window)
        {
            window.Left = this.Left;
            window.Top = this.Top;
            window.Show();
            this.Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        // ── CHARGEMENT DONNÉES ────────────────────────────────────
        private async void LoadStatsAsync()
        {
            TxtStatus.Visibility = Visibility.Visible;
            try
            {
                await LoadSaisonStats();
                await LoadTopPilotes();
                await LoadTopConstructeurs();
                await LoadDernieresCourses();
                TxtStatus.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                TxtStatus.Text = "❌ " + ex.Message;
            }
        }

        private async System.Threading.Tasks.Task LoadSaisonStats()
        {
            var response = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/drivers-championship?limit=30");
            var json = JObject.Parse(response);
            var array = json["drivers_championship"] as JArray;

            int saison = json["season"]?.ToObject<int>() ?? DateTime.Now.Year;
            TxtSaison.Text = saison.ToString();
            TxtNbPilotes.Text = (array?.Count ?? 20).ToString();

            var resRaces = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/last");
            var jsonRaces = JObject.Parse(resRaces);
            int round = jsonRaces["race"]?[0]?["round"]?.ToObject<int>() ?? 0;
            TxtNbCourses.Text = round.ToString() + "/24";
        }

        private async System.Threading.Tasks.Task LoadTopPilotes()
        {
            var response = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/drivers-championship?limit=10");
            var json = JObject.Parse(response);
            var array = json["drivers_championship"] as JArray;
            if (array == null) return;

            var items = new List<StatPiloteItem>();
            double maxPoints = 1;

            foreach (var item in array)
            {
                int pos = item["position"]?.ToObject<int>() ?? 99;
                if (pos > 10) continue;
                double pts = item["points"]?.ToObject<double>() ?? 0;
                if (pos == 1) maxPoints = pts > 0 ? pts : 1;

                items.Add(new StatPiloteItem
                {
                    Position = pos,
                    Name = item["driver"]?["name"]?.ToString() ?? "—",
                    Surname = item["driver"]?["surname"]?.ToString() ?? "—",
                    ShortName = item["driver"]?["shortName"]?.ToString() ?? "—",
                    TeamId = item["teamId"]?.ToString() ?? "",
                    Points = pts,
                    Wins = item["wins"]?.ToObject<int>() ?? 0,
                    MaxPoints = maxPoints
                });
            }

            items.Sort((a, b) => a.Position.CompareTo(b.Position));
            foreach (var it in items) it.MaxPoints = maxPoints;
            ListPilotes.ItemsSource = items;
        }

        private async System.Threading.Tasks.Task LoadTopConstructeurs()
        {
            var response = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/constructors-championship?limit=10");
            var json = JObject.Parse(response);
            var array = json["constructors_championship"] as JArray;
            if (array == null) return;

            var items = new List<StatConstructeurItem>();
            double maxPoints = 1;

            foreach (var item in array)
            {
                int pos = item["position"]?.ToObject<int>() ?? 99;
                if (pos > 10) continue;
                double pts = item["points"]?.ToObject<double>() ?? 0;
                if (pos == 1) maxPoints = pts > 0 ? pts : 1;

                items.Add(new StatConstructeurItem
                {
                    Position = pos,
                    TeamId = item["teamId"]?.ToString() ?? "",
                    TeamName = item["team"]?["teamName"]?.ToString() ?? "—",
                    Points = pts,
                    Wins = item["wins"]?.ToObject<int>() ?? 0,
                    MaxPoints = maxPoints
                });
            }

            items.Sort((a, b) => a.Position.CompareTo(b.Position));
            foreach (var it in items) it.MaxPoints = maxPoints;
            ListConstructeurs.ItemsSource = items;
        }

        private async System.Threading.Tasks.Task LoadDernieresCourses()
        {
            var response = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/last");
            var json = JObject.Parse(response);
            var races = json["race"] as JArray;
            if (races == null || races.Count == 0) return;

            var race = races[0];
            TxtDerniereRace.Text = race["raceName"]?.ToString() ?? "—";
            TxtDerniereRound.Text = "Round " + (race["round"]?.ToString() ?? "—");
            TxtDerniereCircuit.Text = race["circuit"]?["circuitName"]?.ToString() ?? "—";
            TxtDerniereDate.Text = race["schedule"]?["race"]?["date"]?.ToString() ?? "—";

            var winner = race["winner"];
            var teamWinner = race["teamWinner"];
            if (winner != null && winner.Type != JTokenType.Null)
            {
                TxtDerniereWinner.Text = (winner["name"]?.ToString() ?? "—") + " " +
                                         (winner["surname"]?.ToString() ?? "");
                TxtDerniereTeam.Text = teamWinner?["teamName"]?.ToString() ?? "—";
            }
            else
            {
                TxtDerniereWinner.Text = "Résultats en attente";
                TxtDerniereTeam.Text = "—";
            }

            var nextRes = await _httpClient.GetStringAsync(
                "https://f1api.dev/api/current/next");
            var jsonNext = JObject.Parse(nextRes);
            var nextRaces = jsonNext["race"] as JArray;
            if (nextRaces != null && nextRaces.Count > 0)
            {
                var next = nextRaces[0];
                TxtProchainRace.Text = next["raceName"]?.ToString() ?? "—";
                TxtProchainRound.Text = "Round " + (next["round"]?.ToString() ?? "—");
                TxtProchainCircuit.Text = next["circuit"]?["circuitName"]?.ToString() ?? "—";
                TxtProchainDate.Text = next["schedule"]?["race"]?["date"]?.ToString() ?? "—";

                string dateStr = next["schedule"]?["race"]?["date"]?.ToString() ?? "";
                if (DateTime.TryParse(dateStr, out DateTime raceDate))
                {
                    int days = (int)(raceDate - DateTime.Now).TotalDays;
                    TxtProchainDays.Text = days > 0 ? $"Dans {days} jours" : "Ce week-end !";
                }
            }
        }

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Accueil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOUTONS CATÉGORIES ────────────────────────────────────
        private void BtnPilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        private void BtnConstructeurs_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ConstructorPage());
        private void BtnCourses_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void BtnCircuits_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CircuitsPage());

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
        {
            var live = new LivePage();
            live.Owner = this;
            live.ShowDialog();
        }
        private void Nav_Profil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ProfilPage());
    }

    // ── MODÈLES ───────────────────────────────────────────────────
    public class StatPiloteItem
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ShortName { get; set; }
        public string TeamId { get; set; }
        public double Points { get; set; }
        public int Wins { get; set; }
        public double MaxPoints { get; set; }

        public double BarWidth => MaxPoints > 0 ? (Points / MaxPoints) * 260 : 0;

        public string PositionColor => Position == 1 ? "#FFD700"
            : Position == 2 ? "#C0C0C0"
            : Position == 3 ? "#CD7F32" : "#555555";

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
                    default: return "#E8002D";
                }
            }
        }
    }

    public class StatConstructeurItem
    {
        public int Position { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public double Points { get; set; }
        public int Wins { get; set; }
        public double MaxPoints { get; set; }

        public double BarWidth => MaxPoints > 0 ? (Points / MaxPoints) * 260 : 0;

        public string PositionColor => Position == 1 ? "#FFD700"
            : Position == 2 ? "#C0C0C0"
            : Position == 3 ? "#CD7F32" : "#555555";

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
                    default: return "#E8002D";
                }
            }
        }
    }
}