using System.Windows;
using System.Windows.Input;
using F1_Live_Hub.Services;

namespace F1_Live_Hub.Views
{
    public partial class LoginPage : Window
    {
        private readonly AuthService _authService;

        public LoginPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        private void ConnecterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = TxtUsername.Text?.Trim() ?? "";
            var password = PwdPassword.Password ?? "";

            TxtErrorBorder.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Veuillez entrer un nom d'utilisateur.");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Veuillez entrer un mot de passe.");
                return;
            }

            if (_authService.Login(username, password))
            {
                _authService.SaveCurrentUser(username);
                var accueil = new AccueilPage();
                accueil.Left = this.Left;
                accueil.Top = this.Top;
                accueil.Show();
                this.Close();
            }
            else
            {
                ShowError("Identifiants incorrects.");
            }
        }

        private void ShowError(string msg)
        {
            TxtError.Text = "⚠ " + msg;
            TxtErrorBorder.Visibility = Visibility.Visible;
        }

        private void CreerProfil_Click(object sender, MouseButtonEventArgs e)
        {
            var register = new RegisterPage();
            register.Left = this.Left;
            register.Top = this.Top;
            register.Show();
            this.Close();
        }
    }
}