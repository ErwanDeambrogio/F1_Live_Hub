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
        {
            LoadLastRaceAsync();
        }

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
            {
                var blank = new Course { IsLoading = loading };
                this.DataContext = blank;
            }
        }

        private Course ErrorCourse(string msg) =>
            new Course { HasError = true, ErrorMessage = msg };

        private void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }

        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        // ONGLETS
        private void Tab_Meteo_Click(object sender, MouseButtonEventArgs e) { }

        // NAVIGATION
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Standings_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Nav_Calendar_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CircuitsPage());
        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            var win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}
