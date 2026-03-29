using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using F1_Live_Hub.Models;

namespace F1_Live_Hub.Views
{
    public partial class CircuitsPage : Window
    {
        private HttpClient _client = new HttpClient();
        private Circuit _circuitSelectionne;

        public CircuitsPage()
        {
            InitializeComponent();
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

        private async void OnSearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string texte = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(texte))
            {
                EmptyState.Visibility = Visibility.Visible;
                CircuitsCollection.Visibility = Visibility.Collapsed;
                NoResultPanel.Visibility = Visibility.Collapsed;
                return;
            }
            LoadingSpinner.Visibility = Visibility.Visible;
            List<Circuit> resultats = await ChercherCircuitsAsync(texte);
            LoadingSpinner.Visibility = Visibility.Collapsed;
            if (resultats == null || resultats.Count == 0)
            {
                EmptyState.Visibility = Visibility.Collapsed;
                CircuitsCollection.Visibility = Visibility.Collapsed;
                NoResultPanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyState.Visibility = Visibility.Collapsed;
                NoResultPanel.Visibility = Visibility.Collapsed;
                CircuitsCollection.Visibility = Visibility.Visible;
                CircuitsCollection.ItemsSource = resultats;
            }
        }

        private async Task<List<Circuit>> ChercherCircuitsAsync(string recherche)
        {
            try
            {
                string url = "https://f1api.dev/api/circuits/search?q=" + Uri.EscapeDataString(recherche);
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Root resulat = JsonConvert.DeserializeObject<Root>(json);
                    return resulat?.circuits;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur réseau : " + ex.Message);
            }
            return null;
        }

        private void OnCircuitSelected(object sender, MouseButtonEventArgs e)
        {
            var border = sender as FrameworkElement;
            var circuit = border?.DataContext as Circuit;
            if (circuit == null) return;
            _circuitSelectionne = circuit;
            HeroName.Text = circuit.circuitName;
            HeroLocation.Text = circuit.city + ", " + circuit.country;
            TxtLength.Text = circuit.circuitLength.HasValue ? circuit.circuitLength.ToString() : "N/A";
            TxtCorners.Text = circuit.numberOfCorners.HasValue ? circuit.numberOfCorners.ToString() : "N/A";
            TxtFirstYear.Text = circuit.firstParticipationYear.HasValue ? circuit.firstParticipationYear.ToString() : "N/A";
            TxtLapRecord.Text = !string.IsNullOrEmpty(circuit.lapRecord) ? circuit.lapRecord : "N/A";
            TxtLapRecordBig.Text = !string.IsNullOrEmpty(circuit.lapRecord) ? circuit.lapRecord : "N/A";
            TxtFastestDriver.Text = circuit.fastestLapDriverId ?? "Inconnu";
            TxtFastestTeam.Text = circuit.fastestLapTeamId ?? "—";
            TxtFastestYear.Text = circuit.fastestLapYear.HasValue ? circuit.fastestLapYear.ToString() : "—";
            TxtLengthDetail.Text = circuit.circuitLength.HasValue ? circuit.circuitLength.ToString() : "N/A";
            TxtCornersDetail.Text = circuit.numberOfCorners.HasValue ? circuit.numberOfCorners.ToString() : "N/A";
            TxtHistoryYear.Text = circuit.firstParticipationYear.HasValue ? circuit.firstParticipationYear.ToString() : "N/A";
            TxtRecordDriver.Text = circuit.fastestLapDriverId ?? "Inconnu";
            TxtRecordTeam.Text = circuit.fastestLapTeamId ?? "—";
            TxtCity.Text = circuit.city ?? "—";
            TxtCountry.Text = circuit.country ?? "—";
            TxtRecordYear.Text = circuit.fastestLapYear.HasValue ? circuit.fastestLapYear.ToString() : "—";
            PanelRecherche.Visibility = Visibility.Collapsed;
            PanelDetail.Visibility = Visibility.Visible;
        }

        private void OnBackClicked(object sender, MouseButtonEventArgs e)
        {
            PanelDetail.Visibility = Visibility.Collapsed;
            PanelRecherche.Visibility = Visibility.Visible;
        }

        private void OnWikipediaClicked(object sender, MouseButtonEventArgs e)
        {
            if (_circuitSelectionne == null || string.IsNullOrEmpty(_circuitSelectionne.url)) return;
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = _circuitSelectionne.url,
                UseShellExecute = true
            });
        }

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Accueil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Tab_Meteo_Click(object sender, MouseButtonEventArgs e) { }

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Stats_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new StatPage());
        private void Nav_Profil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ProfilPage());
        private void Nav_Home_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Nav_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Nav_Saison_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
    }
}