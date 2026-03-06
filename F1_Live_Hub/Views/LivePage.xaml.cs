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
    public partial class LivePage : Window
    {
        public LivePage()
        {
            InitializeComponent();
        }

        private void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }

        // ── BOTTOM NAV ───────────────────────────────────────────

        private void Nav_Standings_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());

        private void Nav_News_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        private void Nav_Calendar_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());

        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        // ── VOIR TOUT ────────────────────────────────────────────

        private void VoirTout_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
    }
}