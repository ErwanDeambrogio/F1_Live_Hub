using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Logique d'interaction pour MeteoPage.xaml
    /// </summary>
    public partial class MeteoPage : Window
    {
        public MeteoPage()

        {
        _: GetBlogdetails();
            InitializeComponent();
        }
    

    public async Task<string> GetBlogdetails()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.openf1.org/v1/weather");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                return "Error: " + response.StatusCode;
            }


        }


    }
}

    

