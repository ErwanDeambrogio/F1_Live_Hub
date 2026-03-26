using System.Windows;
using System.Windows.Controls;
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

        private void ConnecterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = TxtUsername.Text?.Trim() ?? "";
            var password = PwdPassword.Password ?? "";

            TxtError.Visibility = Visibility.Collapsed;

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
                var main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                ShowError("Identifiants incorrects. Vérifiez votre nom et mot de passe.");
            }
        }

        private void ShowError(string msg)
        {
            TxtError.Text = "⚠ " + msg;
            TxtError.Visibility = Visibility.Visible;
        }

        private void CreerProfil_Click(object sender, MouseButtonEventArgs e)
        {
            var register = new RegisterPage();
            register.Show();
            this.Close();
        }
    }
}