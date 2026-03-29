using System.Windows;
using System.Windows.Input;

namespace F1_Live_Hub.Views
{
    public partial class ConstructorPage : Window
    {
        public ConstructorPage()
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
        private void Tab_Accueil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Tab_Pilotes_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Accueil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Nav_Live_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Stats_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new StatPage());
        private void Nav_Course_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new CoursePage());
        private void Nav_Profil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ProfilPage());

        private void ProfilButton_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ProfilPage());
    }
}