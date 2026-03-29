using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;

namespace F1_Live_Hub.Views
{
    public partial class AccueilPage : Window
    {
        private DispatcherTimer _countdownTimer;
        private readonly HttpClient _httpClient = new HttpClient();
        private DateTime _nextRaceDate;

        public AccueilPage()
        {
            InitializeComponent();
            LoadDataAsync();
        }

        private void OpenWindow(Window window)
        {
            window.Left = this.Left;
            window.Top = this.Top;
            window.Show();
            this.Close();
        }

        // ── CHARGEMENT DONNÉES ────────────────────────────────────
        private async void LoadDataAsync()
        {
            try
            {
                await LoadNextRace();
                await LoadChampionship();
                await LoadFeaturedDriver();
            }
            catch { }
        }

        private async System.Threading.Tasks.Task LoadNextRace()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    "https://f1api.dev/api/current/next");
                var json = JObject.Parse(response);
                var races = json["race"] as JArray;
                if (races == null || races.Count == 0) return;

                var race = races[0];
                string raceName = race["raceName"]?.ToString() ?? "—";
                string circuit = race["circuit"]?["circuitName"]?.ToString() ?? "—";
                string city = race["circuit"]?["city"]?.ToString() ?? "—";
                string country = race["circuit"]?["country"]?.ToString() ?? "—";
                int round = race["round"]?.ToObject<int>() ?? 0;
                string dateStr = race["schedule"]?["race"]?["date"]?.ToString() ?? "";

                TxtNextRaceName.Text = raceName.ToUpper();
                TxtNextRaceCircuit.Text = "🏁 " + circuit;
                TxtNextRaceLocation.Text = "📍 " + city + ", " + country;
                TxtNextRaceRound.Text = "Round " + round;

                if (DateTime.TryParse(dateStr, out DateTime raceDate))
                {
                    _nextRaceDate = raceDate;
                    TxtNextRaceDate.Text = "📅 " + raceDate.ToString("dd MMM yyyy");
                    StartCountdown();
                }
            }
            catch { }
        }

        private async System.Threading.Tasks.Task LoadChampionship()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    "https://f1api.dev/api/current/drivers-championship?limit=3");
                var json = JObject.Parse(response);
                var array = json["drivers_championship"] as JArray;
                if (array == null) return;

                foreach (var item in array)
                {
                    int pos = item["position"]?.ToObject<int>() ?? 99;
                    string name = item["driver"]?["name"]?.ToString() ?? "—";
                    string surname = item["driver"]?["surname"]?.ToString() ?? "—";
                    string shortName = item["driver"]?["shortName"]?.ToString() ?? "—";
                    string teamId = item["teamId"]?.ToString() ?? "";
                    double pts = item["points"]?.ToObject<double>() ?? 0;
                    int wins = item["wins"]?.ToObject<int>() ?? 0;
                    string teamName = item["team"]?["teamName"]?.ToString() ?? "—";

                    string teamColor = GetTeamColor(teamId);

                    switch (pos)
                    {
                        case 1:
                            P1Num.Text = "01";
                            P1Color.Background = new System.Windows.Media.SolidColorBrush(
                                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(teamColor));
                            P1Name.Text = name[0] + ". " + surname;
                            P1Team.Text = teamName;
                            P1Pts.Text = pts + " PTS";
                            break;
                        case 2:
                            P2Num.Text = "02";
                            P2Color.Background = new System.Windows.Media.SolidColorBrush(
                                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(teamColor));
                            P2Name.Text = name[0] + ". " + surname;
                            P2Team.Text = teamName;
                            P2Pts.Text = pts + " PTS";
                            break;
                        case 3:
                            P3Num.Text = "03";
                            P3Color.Background = new System.Windows.Media.SolidColorBrush(
                                (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(teamColor));
                            P3Name.Text = name[0] + ". " + surname;
                            P3Team.Text = teamName;
                            P3Pts.Text = pts + " PTS";
                            break;
                    }
                }
            }
            catch { }
        }

        private async System.Threading.Tasks.Task LoadFeaturedDriver()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    "https://f1api.dev/api/current/drivers-championship?limit=1");
                var json = JObject.Parse(response);
                var array = json["drivers_championship"] as JArray;
                if (array == null || array.Count == 0) return;

                var leader = array[0];
                string name = leader["driver"]?["name"]?.ToString() ?? "—";
                string surname = leader["driver"]?["surname"]?.ToString() ?? "—";
                string shortName = leader["driver"]?["shortName"]?.ToString() ?? "—";
                string teamId = leader["teamId"]?.ToString() ?? "";
                string teamName = leader["team"]?["teamName"]?.ToString() ?? "—";
                string country = leader["driver"]?["country"]?.ToString() ?? "";
                double pts = leader["points"]?.ToObject<double>() ?? 0;
                int wins = leader["wins"]?.ToObject<int>() ?? 0;

                TxtFeaturedName.Text = name.ToUpper();
                TxtFeaturedSurname.Text = surname.ToUpper();
                TxtFeaturedTeam.Text = teamName;
                TxtFeaturedShort.Text = shortName;
                TxtFeaturedPts.Text = pts + " PTS";
                TxtFeaturedWins.Text = wins + " VICTOIRES";
            }
            catch { }
        }

        private string GetTeamColor(string teamId)
        {
            switch (teamId?.ToLower())
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

        // ── COMPTE À REBOURS ──────────────────────────────────────
        private void StartCountdown()
        {
            _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _countdownTimer.Tick += (s, e) => UpdateCountdown();
            _countdownTimer.Start();
            UpdateCountdown();
        }

        private void UpdateCountdown()
        {
            var diff = _nextRaceDate - DateTime.Now;
            if (diff.TotalSeconds <= 0)
            {
                _countdownTimer?.Stop();
                CountDays.Text = "00";
                CountHours.Text = "00";
                CountMins.Text = "00";
                CountSecs.Text = "00";
                return;
            }
            CountDays.Text = ((int)diff.TotalDays).ToString("D2");
            CountHours.Text = diff.Hours.ToString("D2");
            CountMins.Text = diff.Minutes.ToString("D2");
            CountSecs.Text = diff.Seconds.ToString("D2");
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
        {
            var live = new LivePage();
            live.Owner = this;
            live.ShowDialog();
        }
        private void Nav_Stats_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new StatPage());
        private void Nav_Profil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ProfilPage());

        // ── HIGHLIGHTS ───────────────────────────────────────────
        private void BtnHighlight1_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.youtube.com/results?search_query=f1+race+highlights+2025",
                UseShellExecute = true
            });
        }

        private void BtnHighlight2_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.youtube.com/F1/videos",
                UseShellExecute = true
            });
        }
    }
}