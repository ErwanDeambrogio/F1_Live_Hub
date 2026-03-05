using System.Windows;
using System.Windows.Input;

namespace F1_Live_Hub.Views
{
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void CreerCompte_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RetourConnexion_Click(object sender, MouseButtonEventArgs e)
        {
            var login = new LoginPage();
            login.Show();
            this.Close();
        }
    }
}