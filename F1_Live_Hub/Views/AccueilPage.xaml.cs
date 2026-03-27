using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace F1_Live_Hub.Views
{
    public partial class AccueilPage : Window
    {
        private DispatcherTimer _timer;
        private DateTime _nextRace = new DateTime(2025, 7, 5, 14, 0, 0);

        public AccueilPage()
        {
            InitializeComponent();
            StartCountdown();
        }

        private void OpenWindow(Window window)
        {
            window.Left = this.Left;
            window.Top = this.Top;
            window.Show();
            this.Close();
        }

        private void StartCountdown()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => UpdateCountdown();
            _timer.Start();
            UpdateCountdown();
        }

        private void UpdateCountdown()
        {
            var diff = _nextRace - DateTime.Now;
            if (diff.TotalSeconds <= 0) { _timer.Stop(); return; }
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
        private void Tab_Classement_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Tab_Pilotes_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Live_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Stats_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new StatPage());
        private void Nav_Course_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new CoursePage());
        private void Nav_Profil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ProfilPage());
    }
}