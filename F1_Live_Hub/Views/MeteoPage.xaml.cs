
using F1_Live_Hub.Models;
using F1_Live_Hub.Services; 
using Newtonsoft.Json;
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
                List<MeteoService.Root> liste = JsonConvert.DeserializeObject<List<MeteoService.Root>>(responseBody);
                MeteoService.Root meteo = liste[liste.Count - 1];

                BTN_Tempaire.Text = meteo.air_temperature.ToString("F1") + "°C";
                BTN_Temppist.Text = meteo.track_temperature.ToString("F1") + "°C";
                BTN_Humidité.Text = meteo.humidity + "%";
                BTN_Vent.Text =(meteo.wind_speed * 3.6).ToString("F0") + "Km/h";
                BTN_Pression.Text = meteo.pressure.ToString("F0");
                BTN_Precipitation.Text = meteo.rainfall == 1  ? "Pluie 🌧" : "Sec ☀";


                return responseBody;

              
            }
            else
            {
                return "Error: " + response.StatusCode;
            }


        }


    }
}

    

