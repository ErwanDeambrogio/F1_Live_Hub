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
    /// <summary>
    /// Logique d'interaction pour ClassementPage.xaml
    /// </summary>
    public partial class ClassementPage : Window
    {
        public ClassementPage()
        {
            InitializeComponent();
        }

        private void BtnAnnee_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for year button click
        }

        private void AnneeItem_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for year item selection
            if (sender is Button btn && btn.Tag is string year)
            {
                TxtAnnee.Text = year;
            }
        }

        private void BtnGP_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for GP button click
        }

        private void GPItem_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for GP item selection
            if (sender is Button btn && btn.Tag is string gp)
            {
                TxtGP.Text = gp;
            }
        }

        private void BtnSession_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for session button click
        }

        private void SessionItem_Click(object sender, RoutedEventArgs e)
        {
            // Implementation for session item selection
            if (sender is Button btn && btn.Tag is string session)
            {
                TxtSession.Text = session;
            }
        }
    }
}
