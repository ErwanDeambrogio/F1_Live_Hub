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

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Accueil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new AccueilPage());
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

        // ── FILTRES ──────────────────────────────────────────────
        private void BtnAnnee_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnGP_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnSession_Click(object sender, MouseButtonEventArgs e) { }
    }
}