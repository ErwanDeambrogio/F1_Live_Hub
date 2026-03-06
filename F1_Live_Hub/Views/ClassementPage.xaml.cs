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
    public partial class ClassementPage : Window
    {
        public ClassementPage()
        {
            InitializeComponent();
        }

        // Méthode de navigation identique à AccueilPage
        private void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }

        // ── FILTRES ──────────────────────────────────────────────

        private void BtnAnnee_Click(object sender, MouseButtonEventArgs e)
        {
            // TODO: afficher dropdown année
        }

        private void AnneeItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string year)
            {
                // TODO: Update year display
            }
        }

        private void BtnGP_Click(object sender, MouseButtonEventArgs e)
        {
            // TODO: afficher dropdown GP
        }

        private void GPItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string gp)
            {
                // TODO: Update GP display
            }
        }

        private void BtnSession_Click(object sender, MouseButtonEventArgs e)
        {
            // TODO: afficher dropdown session
        }

        private void SessionItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string session)
            {
                // TODO: Update session display
            }
        }

        // ── TABS ─────────────────────────────────────────────────

        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());

        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        private void Tab_Meteo_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new MeteoPage());

        // ── BOTTOM NAV ───────────────────────────────────────────

        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
        {
            // Déjà sur cette page
        }

        private void Nav_Standings_Click(object sender, MouseButtonEventArgs e)
        {
            // Déjà sur ClassementPage
        }

        private void Nav_News_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        private void Nav_Calendar_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());

        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            // AccueilPage est une Page, pas une Window
            // On réaffiche la MainWindow qui contient le Frame
            Application.Current.MainWindow.Show();
            this.Close();
        }
    }
}