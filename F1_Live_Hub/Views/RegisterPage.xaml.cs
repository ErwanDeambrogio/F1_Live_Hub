using System.Windows;
using System.Windows.Input;
using F1_Live_Hub.Services;

namespace F1_Live_Hub.Views
{
    public partial class RegisterPage : Window
    {
        private readonly AuthService _authService;

        public RegisterPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        private void CreerCompte_Click(object sender, RoutedEventArgs e)
        {
            var username = TxtUsername.Text?.Trim() ?? "";
            var password = PwdPassword.Password ?? "";
            var confirm = PwdConfirm.Password ?? "";

            TxtErrorBorder.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Veuillez entrer un nom d'utilisateur.");
                return;
            }
            if (password.Length < 4)
            {
                ShowError("Le mot de passe doit contenir au moins 4 caractères.");
                return;
            }
            if (password != confirm)
            {
                ShowError("Les mots de passe ne correspondent pas.");
                return;
            }

            if (_authService.Register(username, password))
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
                ShowError("Ce nom d'utilisateur est déjà pris.");
            }
        }

        private void ShowError(string msg)
        {
            TxtError.Text = "⚠ " + msg;
            TxtErrorBorder.Visibility = Visibility.Visible;
        }

        private void RetourConnexion_Click(object sender, MouseButtonEventArgs e)
        {
            var login = new LoginPage();
            login.Left = this.Left;
            login.Top = this.Top;
            login.Show();
            this.Close();
        }
    }
}