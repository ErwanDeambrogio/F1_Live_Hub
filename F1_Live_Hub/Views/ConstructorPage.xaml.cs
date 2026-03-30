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
    // Classe pour l'affichage de chaque constructeur dans la liste
    public class ConstructeurAffiche
    {
        public string Position { get; set; }        // Ex: "01"
        public string NomEquipe { get; set; }       // Ex: "RED BULL RACING"
        public string Points { get; set; }          // Ex: "860"
        public string Victoires { get; set; }       // Ex: "9 🏆"
        public string Ecart { get; set; }           // Ex: "LEADER" ou "-344 pts"
        public string CouleurEquipe { get; set; }   // Couleur de la barre gauche
        public string CouleurPosition { get; set; } // Rouge si top 3, gris sinon
        public string CouleurEcart { get; set; }    // Rouge si leader, gris sinon
    }

    public partial class ConstructorPage : Window
    {
        public ConstructorPage()
        {
            InitializeComponent();
            // On charge le classement dès l'ouverture de la fenêtre
            _ = ChargerClassement();
        }

        // Méthode async qui appelle l'API et remplit la liste
        public async Task ChargerClassement()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://f1api.dev/api/2026/constructors-championship");

                if (response.IsSuccessStatusCode)
                {
                    // On récupère le JSON et on le désérialise avec RootConstructeur
                    string content = await response.Content.ReadAsStringAsync();
                    RootConstructeur classement = JsonConvert.DeserializeObject<RootConstructeur>(content);

                    List<ConstructeurAffiche> liste = new List<ConstructeurAffiche>();
                    double pointsLeader = 0;

                    foreach (ConstructorsChampionship equipe in classement.constructors_championship)
                    {
                        // On sauvegarde les points du leader (position 1)
                        if (equipe.position == 1)
                            pointsLeader = equipe.points;

                        // Calcul de l'écart avec le leader
                        string ecart = equipe.position == 1
                            ? "LEADER"
                            : $"-{pointsLeader - equipe.points} pts";

                        liste.Add(new ConstructeurAffiche
                        {
                            Position = equipe.position.ToString("D2"),
                            NomEquipe = equipe.team.teamName.ToUpper(),
                            Points = equipe.points.ToString(),
                            Victoires = equipe.wins + " 🏆",
                            Ecart = ecart,
                            CouleurEquipe = CouleurParEquipe(equipe.teamId),
                            CouleurPosition = equipe.position <= 3 ? "#E8002D" : "#3A3A55",
                            CouleurEcart = equipe.position == 1 ? "#E8002D" : "#5A5A7A"
                        });
                    }

                    // On envoie la liste à l'ItemsControl du XAML
                    ListeConstructeurs.ItemsSource = liste;
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

        // Retourne la couleur selon l'ID de l'équipe
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
            window.Left = this.Left;
            window.Top = this.Top;
            window.Show();
            this.Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        // ── TABS ─────────────────────────────────────────────────
        private void Tab_Accueil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Tab_Pilotes_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOTTOM NAV ───────────────────────────────────────────
        private void Nav_Accueil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Nav_Live_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new LivePage());
        private void Nav_Stats_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new StatPage());
        private void Nav_Course_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new CoursePage());
        private void Nav_Profil_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ProfilPage());
        private void ProfilButton_Click(object sender, RoutedEventArgs e)
            => OpenWindow(new ProfilPage());
    }
}