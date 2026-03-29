using System;
using System.Windows;
using System.Windows.Input;
using F1_Live_Hub.Models;
using F1_Live_Hub.Services;

namespace F1_Live_Hub.Views
{
    public partial class CoursePage : Window
    {
        private readonly SessionService _sessionService;

        public CoursePage()
        {
            InitializeComponent();
            _sessionService = new SessionService();
            LoadLastRaceAsync();
        }

        private void OpenWindow(Window window)
        {
            window.Left = this.Left;
            window.Top = this.Top;
            window.Show();
            this.Close();
        }

        private async void LoadLastRaceAsync()
        {
            SetLoading(true);
            try
            {
                var course = await _sessionService.GetLastRaceAsync();
                this.DataContext = course ?? ErrorCourse("Aucune donnée reçue.");
            }
            catch (Exception ex)
            {
                this.DataContext = ErrorCourse("Erreur API : " + ex.Message);
            }
            finally { SetLoading(false); }
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            SetLoading(true);
            try
            {
                var course = await _sessionService.GetNextRaceAsync();
                this.DataContext = course ?? ErrorCourse("Aucune donnée reçue.");
            }
            catch (Exception ex)
            {
                this.DataContext = ErrorCourse("Erreur : " + ex.Message);
            }
            finally { SetLoading(false); }
        }

        private void BtnLast_Click(object sender, RoutedEventArgs e)
            => LoadLastRaceAsync();

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtYear.Text?.Trim(), out int year) ||
                !int.TryParse(TxtRound.Text?.Trim(), out int round))
            {
                this.DataContext = ErrorCourse("Année ou round invalide.");
                return;
            }
            SetLoading(true);
            try
            {
                var course = await _sessionService.GetRaceByYearRoundAsync(year, round);
                this.DataContext = course ?? ErrorCourse("Course introuvable.");
            }
            catch (Exception ex)
            {
                this.DataContext = ErrorCourse("Erreur : " + ex.Message);
            }
            finally { SetLoading(false); }
        }

        private void SetLoading(bool loading)
        {
            if (this.DataContext is Course c)
                c.IsLoading = loading;
            else
                this.DataContext = new Course { IsLoading = loading };
        }

        private Course ErrorCourse(string msg) =>
            new Course { HasError = true, ErrorMessage = msg };

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Accueil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Tab_Classement_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Pilotes_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new PilotesPage());

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
        {
            var live = new LivePage();
            live.Owner = this;
            live.ShowDialog();
        }
        private void Nav_Stats_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new StatPage());
        private void Nav_Profil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ProfilPage());
        private void Nav_Accueil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new AccueilPage());
    }
}