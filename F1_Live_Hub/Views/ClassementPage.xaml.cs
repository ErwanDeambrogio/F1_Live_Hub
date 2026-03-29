using F1_Live_Hub.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace F1_Live_Hub.Views
{
    // Ici c'est normal, c'est juste pour l'affichage
    public class PiloteAffiche
    {
        public string Position { get; set; }
        public string NomAffiche { get; set; }
        public string NomEquipe { get; set; }
        public string Points { get; set; }
        public string Ecart { get; set; }
        public string CouleurEquipe { get; set; }
        public string CouleurPosition { get; set; }
        public string CouleurEcart { get; set; }
    }

    public partial class ClassementPage : Window
    {
        public ClassementPage()
        {
            InitializeComponent();
            _ = ChargerClassement();
        }

        public async Task ChargerClassement()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://f1api.dev/api/2026/drivers-championship");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RootClassement classement = JsonConvert.DeserializeObject<RootClassement>(content);

                    List<PiloteAffiche> liste = new List<PiloteAffiche>();
                    double pointsLeader = 0;

                    foreach (DriversChampionship pilote in classement.drivers_championship)
                    {
                        if (pilote.position == 1)
                            pointsLeader = pilote.points;

                        string ecart = pilote.position == 1
                            ? "LEADER"
                            : $"-{pointsLeader - pilote.points} pts";

                        liste.Add(new PiloteAffiche
                        {
                            Position = pilote.position.ToString("D2"),
                            NomAffiche = $"{pilote.driver.name[0]}. {pilote.driver.surname.ToUpper()}",
                            NomEquipe = pilote.team.teamName,
                            Points = pilote.points.ToString(),
                            Ecart = ecart,
                            CouleurEquipe = CouleurParEquipe(pilote.team.teamId),
                            CouleurPosition = pilote.position <= 3 ? "#E8002D" : "#3A3A55",
                            CouleurEcart = pilote.position == 1 ? "#E8002D" : "#5A5A7A"
                        });
                    }

                    ListeClassement.ItemsSource = liste;
                }
                else
                {
                    MessageBox.Show("Erreur HTTP : " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private string CouleurParEquipe(string teamId)
        {
            switch (teamId)
            {
                case "red_bull": return "#3B6FE8";
                case "ferrari": return "#E8002D";
                case "mercedes": return "#00D2BE";
                case "mclaren": return "#FF8000";
                case "aston_martin": return "#358C75";
                case "alpine": return "#0078FF";
                case "williams": return "#005AFF";
                case "rb": return "#1E3A6E";
                case "haas": return "#AAAAAA";
                case "kick_sauber": return "#52E252";
                default: return "#FFFFFF";
            }
        }

        private void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }

        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Standings_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Nav_News_Click(object sender, MouseButtonEventArgs e) { }
        private void Nav_Calendar_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }
        private void BtnAnnee_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnGP_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnSession_Click(object sender, MouseButtonEventArgs e) { }
    }
}