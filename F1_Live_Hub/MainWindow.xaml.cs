using F1_Live_Hub.Services;
using F1_Live_Hub.Views;
using System.Windows;
using System.Windows.Input;

namespace F1_Live_Hub
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var auth = new AuthService();
            if (auth.IsLoggedIn())
            {
                var accueil = new AccueilPage();
                accueil.Left = this.Left;
                accueil.Top = this.Top;
                accueil.Show();
            }
            else
            {
                var login = new LoginPage();
                login.Left = this.Left;
                login.Top = this.Top;
                login.Show();
            }
            this.Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) DragMove();
        }

        private void CloseButton_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}