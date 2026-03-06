using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace F1_Live_Hub.Views
{
    public partial class AccueilPage : Page
    {
        public AccueilPage()
        {
            InitializeComponent();
        }

        private void OpenWindow(Window window)
        {
            window.Show();
            Application.Current.MainWindow.Hide();
        }

        private void ProfilButton_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new LoginPage());

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

        private void ProfilNav_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new LoginPage());
    }
}