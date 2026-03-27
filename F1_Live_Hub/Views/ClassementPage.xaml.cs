using System.Windows;
using System.Windows.Input;

namespace F1_Live_Hub.Views
{
    public partial class ClassementPage : Window
    {
        public ClassementPage()
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

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());

        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());

        private void Nav_Standings_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());

        private void Nav_News_Click(object sender, MouseButtonEventArgs e) { }

        private void Nav_Calendar_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            var main = Application.Current.MainWindow;
            main.Left = this.Left;
            main.Top = this.Top;
            main.Show();
            this.Close();
        }

        private void BtnAnnee_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnGP_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnSession_Click(object sender, MouseButtonEventArgs e) { }
    }
}