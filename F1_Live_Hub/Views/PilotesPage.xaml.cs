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
    public partial class PilotesPage : Window
    {
        public PilotesPage()
        {
            InitializeComponent();
        }

        private void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }

        // ── TABS ─────────────────────────────────────────────────

        private void Tab_Classement_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());

        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        
        // ── BOTTOM NAV ───────────────────────────────────────────

        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());

        private void Nav_Saison_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());

        private void Nav_Profil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ProfilPage());
    }
}