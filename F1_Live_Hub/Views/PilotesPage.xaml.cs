using F1_Live_Hub.Models;
using F1_Live_Hub.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace F1_Live_Hub.Views
{
    public partial class PilotesPage : Window
    {
        private readonly F1ApiService _apiService;
        private readonly FavoritesService _favService;
        private readonly HttpClient _httpClient = new HttpClient();
        private List<Pilote> _allPilotes = new List<Pilote>();
        private bool _showFavoritesOnly = false;

        public PilotesPage()
        {
            InitializeComponent();
            _apiService = new F1ApiService();
            _favService = new FavoritesService();
            LoadDriversAsync();
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

        private async void LoadDriversAsync()
        {
            TxtStatus.Text = "⏳ Chargement...";
            TxtStatus.Visibility = Visibility.Visible;
            DetailPanel.Visibility = Visibility.Collapsed;
            try
            {
                _allPilotes = await _apiService.GetCurrentDriversAsync();
                foreach (var p in _allPilotes)
                    p.IsFavorite = _favService.IsFavorite(p.DriverId);
                TxtStatus.Visibility = Visibility.Collapsed;
                RefreshList();
                if (_allPilotes.Count > 0)
                    await LoadDriverDetail(_allPilotes[0]);
            }
            catch (Exception ex)
            {
                TxtStatus.Text = "❌ " + ex.Message;
                TxtStatus.Visibility = Visibility.Visible;
            }
        }

        private void RefreshList()
        {
            var q = TxtSearch.Text?.ToLower() ?? "";
            var filtered = new List<Pilote>();
            foreach (var p in _allPilotes)
            {
                bool matchSearch = string.IsNullOrWhiteSpace(q) ||
                    p.Name.ToLower().Contains(q) ||
                    p.Surname.ToLower().Contains(q) ||
                    p.ShortName.ToLower().Contains(q);
                bool matchFav = !_showFavoritesOnly || p.IsFavorite;
                if (matchSearch && matchFav) filtered.Add(p);
            }
            filtered.Sort((a, b) =>
            {
                if (a.IsFavorite && !b.IsFavorite) return -1;
                if (!a.IsFavorite && b.IsFavorite) return 1;
                return string.Compare(a.Surname, b.Surname, StringComparison.OrdinalIgnoreCase);
            });
            DriversList.ItemsSource = filtered;

            int favCount = 0;
            foreach (var p in _allPilotes)
                if (p.IsFavorite) favCount++;

            BtnFavFilter.Content = _showFavoritesOnly
                ? string.Format("★ Favoris ({0}) ▲", favCount)
                : string.Format("★ Favoris ({0}) ▼", favCount);

            FavBanner.Visibility = _showFavoritesOnly
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private async System.Threading.Tasks.Task LoadDriverDetail(Pilote piloteBase)
        {
            DetailPanel.Visibility = Visibility.Collapsed;
            TxtDetailStatus.Visibility = Visibility.Collapsed;
            try
            {
                var pilote = await _apiService.GetDriverDetailAsync(piloteBase.DriverId);
                pilote.IsFavorite = _favService.IsFavorite(pilote.DriverId);
                pilote.PhotoUrl = await GetWikipediaPhotoAsync(pilote.Url);
                DetailPanel.DataContext = pilote;
                ResultsList.ItemsSource = pilote.RaceResults;
                UpdateFavButton(pilote.IsFavorite);
                DetailPanel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                TxtDetailStatus.Text = "❌ " + ex.Message;
                TxtDetailStatus.Visibility = Visibility.Visible;
            }
        }

        private void UpdateFavButton(bool isFav)
        {
            BtnFavDetail.Content = isFav ? "★ Retirer des favoris" : "☆ Ajouter aux favoris";
            BtnFavDetail.Tag = isFav ? "active" : "inactive";
        }

        private void BtnFavorite_Click(object sender, RoutedEventArgs e)
        {
            var pilote = DetailPanel.DataContext as Pilote;
            if (pilote == null) return;
            _favService.Toggle(pilote.DriverId);
            pilote.IsFavorite = _favService.IsFavorite(pilote.DriverId);
            var match = _allPilotes.Find(p => p.DriverId == pilote.DriverId);
            if (match != null) match.IsFavorite = pilote.IsFavorite;
            UpdateFavButton(pilote.IsFavorite);
            RefreshList();
        }

        private void BtnFavFilter_Click(object sender, RoutedEventArgs e)
        {
            _showFavoritesOnly = !_showFavoritesOnly;
            RefreshList();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
            => RefreshList();

        private async void DriversList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DriversList.SelectedItem is Pilote p)
                await LoadDriverDetail(p);
        }

        private async System.Threading.Tasks.Task<string> GetWikipediaPhotoAsync(string wikiUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(wikiUrl)) return "";
                var uri = new Uri(wikiUrl);
                var title = Uri.UnescapeDataString(uri.Segments[uri.Segments.Length - 1]);
                var response = await _httpClient.GetStringAsync(
                    "https://en.wikipedia.org/api/rest_v1/page/summary/" + title);
                var json = JObject.Parse(response);
                return json["thumbnail"]?["source"]?.ToString() ?? "";
            }
            catch { return ""; }
        }

        private void BtnWikipedia_Click(object sender, RoutedEventArgs e)
        {
            var p = DetailPanel.DataContext as Pilote;
            if (p != null && !string.IsNullOrEmpty(p.Url))
                Process.Start(new ProcessStartInfo(p.Url) { UseShellExecute = true });
        }

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Tab_Circuits_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CircuitsPage());

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
        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Nav_Saison_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Tab_Accueil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new AccueilPage());
    }
}