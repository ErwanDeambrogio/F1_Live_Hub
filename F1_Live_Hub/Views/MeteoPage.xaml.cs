using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace F1_Live_Hub.Views
{
    public partial class MeteoPage : Window
    {
        public MeteoPage()
        {
            InitializeComponent();
        }

        private void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }

        // ── TABS ─────────────────────────────────────────────────

        private void Tab_Stats_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new StatPage());

        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());

        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOTTOM NAV ───────────────────────────────────────────

        private void Nav_Home_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void Nav_Timing_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());

        private void Nav_Track_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        private void Nav_Profil_Click(object sender, MouseButtonEventArgs e)
        {
            var auth = new F1_Live_Hub.Services.AuthService();
            if (auth.IsLoggedIn())
                OpenWindow(new ProfilPage());
            else
                OpenWindow(new LoginPage());
        }
    }
}