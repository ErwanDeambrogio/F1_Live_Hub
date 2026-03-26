using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using F1_Live_Hub.Services;

namespace F1_Live_Hub.Views
{
    public partial class AccueilPage : Page
    {
        private readonly AuthService _authService;

        public AccueilPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void OpenWindow(Window window)
        {
            window.Show();
            Application.Current.MainWindow.Hide();
        }

        // ── Navigue vers Profil ou Login selon la session ─────────
        private void NavigateToProfil()
        {
            if (_authService.IsLoggedIn())
                OpenWindow(new ProfilPage());
            else
                OpenWindow(new LoginPage());
        }

        private void ProfilButton_Click(object sender, RoutedEventArgs e)
            => NavigateToProfil();

        private void ProfilNav_Click(object sender, RoutedEventArgs e)
            => NavigateToProfil();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                Application.Current.MainWindow.DragMove();
        }

        private void Classement_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ClassementPage());

        private void Pilotes_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new PilotesPage());

        private void Course_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new CoursePage());

        private void Live_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new LivePage());

        private void Stats_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new StatPage());
    }
}