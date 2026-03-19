using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using F1_Live_Hub.Models;
using F1_Live_Hub.Services;
using Newtonsoft.Json.Linq;

namespace F1_Live_Hub.Views
{
    public partial class PilotesPage : Window
    {
        private readonly F1ApiService _apiService;
        private readonly HttpClient _httpClient = new HttpClient();
        private List<Pilote> _allPilotes = new List<Pilote>();

        public PilotesPage()
        {
            InitializeComponent();
            _apiService = new F1ApiService();
            LoadDriversAsync();
        }

        private async void LoadDriversAsync()
        {
            TxtStatus.Text = "⏳ Chargement...";
            TxtStatus.Visibility = Visibility.Visible;
            DetailPanel.Visibility = Visibility.Collapsed;
            try
            {
                _allPilotes = await _apiService.GetCurrentDriversAsync();
                DriversList.ItemsSource = _allPilotes;
                TxtStatus.Visibility = Visibility.Collapsed;
                if (_allPilotes.Count > 0)
                    await LoadDriverDetail(_allPilotes[0]);
            }
            catch (Exception ex)
            {
                TxtStatus.Text = "❌ " + ex.Message;
                TxtStatus.Visibility = Visibility.Visible;
            }
        }

        private async System.Threading.Tasks.Task LoadDriverDetail(Pilote piloteBase)
        {
            // Cacher le détail pendant le chargement sans montrer de texte
            DetailPanel.Visibility = Visibility.Collapsed;
            TxtDetailStatus.Visibility = Visibility.Collapsed;

            try
            {
                var pilote = await _apiService.GetDriverDetailAsync(piloteBase.DriverId);

                // Charger la photo Wikipedia
                pilote.PhotoUrl = await GetWikipediaPhotoAsync(pilote.Url);

                DetailPanel.DataContext = pilote;
                ResultsList.ItemsSource = pilote.RaceResults;
                DetailPanel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                TxtDetailStatus.Text = "❌ " + ex.Message;
                TxtDetailStatus.Visibility = Visibility.Visible;
            }
        }

        // ── Récupère la photo depuis l'API Wikipedia ──────────────
        private async System.Threading.Tasks.Task<string> GetWikipediaPhotoAsync(string wikiUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(wikiUrl)) return "";

                // Extrait le titre de l'article depuis l'URL Wikipedia
                var uri = new Uri(wikiUrl);
                var title = uri.Segments[uri.Segments.Length - 1];
                title = Uri.UnescapeDataString(title);

                var apiUrl = $"https://en.wikipedia.org/api/rest_v1/page/summary/{title}";
                var response = await _httpClient.GetStringAsync(apiUrl);
                var json = JObject.Parse(response);

                return json["thumbnail"]?["source"]?.ToString() ?? "";
            }
            catch
            {
                return "";
            }
        }

        private async void DriversList_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (DriversList.SelectedItem is Pilote p)
                await LoadDriverDetail(p);
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var q = TxtSearch.Text?.ToLower() ?? "";
            DriversList.ItemsSource = string.IsNullOrWhiteSpace(q)
                ? _allPilotes
                : _allPilotes.FindAll(p =>
                    p.Name.ToLower().Contains(q) ||
                    p.Surname.ToLower().Contains(q) ||
                    p.ShortName.ToLower().Contains(q));
        }

        private void BtnWikipedia_Click(object sender, RoutedEventArgs e)
        {
            if (DetailPanel.DataContext is Pilote p && !string.IsNullOrEmpty(p.Url))
                Process.Start(new ProcessStartInfo(p.Url) { UseShellExecute = true });
        }

        private void OpenWindow(Window window) { window.Show(); this.Close(); }

        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Tab_Meteo_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new MeteoPage());
        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Saison_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Nav_Profil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ProfilPage());
    }
}