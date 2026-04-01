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
    // ── Classe d'affichage ────────────────────────────────────────
    // Cette classe sert uniquement à afficher les données dans le XAML
    // via les {Binding ...}. Elle n'est pas liée directement à l'API.
    public class PiloteAffiche
    {
        public string Position { get; set; }        // Ex: "01", "02"...
        public string NomAffiche { get; set; }       // Ex: "M. VERSTAPPEN"
        public string NomEquipe { get; set; }        // Ex: "Red Bull Racing"
        public string Points { get; set; }           // Ex: "575"
        public string Ecart { get; set; }            // Ex: "LEADER" ou "-12 pts"
        public string CouleurEquipe { get; set; }    // Ex: "#3B6FE8" pour Red Bull
        public string CouleurPosition { get; set; }  // Rouge si top 3, gris sinon
        public string CouleurEcart { get; set; }     // Rouge si leader, gris sinon
    }

    public partial class ClassementPage : Window
    {
        // ── Constructeur ──────────────────────────────────────────
        // Appelé automatiquement quand on ouvre la page
        public ClassementPage()
        {
            InitializeComponent();

            // On lance le chargement du classement dès l'ouverture
            // Le _ = signifie qu'on ignore le retour de la tâche async
            _ = ChargerClassement();
        }

        // ── Chargement du classement depuis l'API ─────────────────
        // async = la méthode peut attendre sans bloquer l'interface
        // Task = type de retour pour une méthode async
        public async Task ChargerClassement()
        {
            try
            {
                // Création du client HTTP pour faire des requêtes web
                HttpClient client = new HttpClient();

                // Appel à l'API F1 pour récupérer le classement 2026
                HttpResponseMessage response = await client.GetAsync(
                    "https://f1api.dev/api/2026/drivers-championship");

                // Si la requête a réussi (code 200)
                if (response.IsSuccessStatusCode)
                {
                    // On lit le contenu JSON de la réponse
                    string content = await response.Content.ReadAsStringAsync();

                    // On convertit le JSON en objet C# (désérialisation)
                    RootClassement classement = JsonConvert.DeserializeObject<RootClassement>(content);

                    // Liste qui contiendra les pilotes formatés pour l'affichage
                    List<PiloteAffiche> liste = new List<PiloteAffiche>();

                    // Points du leader pour calculer les écarts
                    double pointsLeader = 0;

                    // On parcourt chaque pilote du classement
                    foreach (DriversChampionship pilote in classement.drivers_championship)
                    {
                        // On sauvegarde les points du pilote en position 1
                        if (pilote.position == 1)
                            pointsLeader = pilote.points;

                        // Calcul de l'écart avec le leader
                        // Si position 1 → "LEADER", sinon → "-X pts"
                        string ecart = pilote.position == 1
                            ? "LEADER"
                            : $"-{pointsLeader - pilote.points} pts";

                        // On ajoute le pilote formaté à la liste
                        liste.Add(new PiloteAffiche
                        {
                            // Position sur 2 chiffres : 1 → "01", 10 → "10"
                            Position = pilote.position.ToString("D2"),

                            // Prénom abrégé + NOM EN MAJUSCULES : "M. VERSTAPPEN"
                            NomAffiche = $"{pilote.driver.name[0]}. {pilote.driver.surname.ToUpper()}",

                            // Nom de l'équipe
                            NomEquipe = pilote.team.teamName,

                            // Points du pilote
                            Points = pilote.points.ToString(),

                            // Écart calculé plus haut
                            Ecart = ecart,

                            // Couleur de la barre gauche selon l'équipe
                            CouleurEquipe = CouleurParEquipe(pilote.team.teamId),

                            // Rouge si top 3, gris foncé sinon
                            CouleurPosition = pilote.position <= 3 ? "#E8002D" : "#3A3A55",

                            // Rouge si leader, gris sinon
                            CouleurEcart = pilote.position == 1 ? "#E8002D" : "#5A5A7A"
                        });
                    }

                    // On envoie la liste au contrôle XAML "ListeClassement"
                    // C'est lui qui affiche les cartes pilotes
                    ListeClassement.ItemsSource = liste;
                }
                else
                {
                    // Affiche le code d'erreur HTTP si la requête a échoué
                    MessageBox.Show("Erreur HTTP : " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Affiche l'erreur si quelque chose s'est mal passé
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        // ── Couleur par équipe ────────────────────────────────────
        // Retourne la couleur officielle de l'équipe selon son ID
        private string CouleurParEquipe(string teamId)
        {
            switch (teamId)
            {
                case "red_bull": return "#3B6FE8"; // Bleu Red Bull
                case "ferrari": return "#E8002D"; // Rouge Ferrari
                case "mercedes": return "#00D2BE"; // Vert Mercedes
                case "mclaren": return "#FF8000"; // Orange McLaren
                case "aston_martin": return "#358C75"; // Vert Aston Martin
                case "alpine": return "#0078FF"; // Bleu Alpine
                case "williams": return "#005AFF"; // Bleu Williams
                case "rb": return "#1E3A6E"; // Bleu foncé RB
                case "haas": return "#AAAAAA"; // Gris Haas
                case "kick_sauber": return "#52E252"; // Vert Kick Sauber
                default: return "#FFFFFF"; // Blanc par défaut
            }
        }

        // ── Navigation ────────────────────────────────────────────
        // Ouvre une nouvelle fenêtre à la même position que celle actuelle
        private void OpenWindow(Window window)
        {
            window.Left = this.Left;
            window.Top = this.Top;
            window.Show();
            this.Close(); // Ferme la fenêtre actuelle
        }

        // Permet de déplacer la fenêtre en cliquant sur le header
        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        // ── TABS ──────────────────────────────────────────────────
        private void Tab_Accueil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new AccueilPage());
        private void Tab_Pilotes_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new PilotesPage());
        private void Tab_Course_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());

        // ── BOTTOM NAV ────────────────────────────────────────────
        private void Nav_Live_Click(object sender, MouseButtonEventArgs e)
        {
            // La LivePage s'ouvre en popup par-dessus la page actuelle
            var live = new LivePage();
            live.Owner = this;
            live.ShowDialog();
        }
        private void Nav_Stats_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new StatPage());
        private void Nav_Profil_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ProfilPage());
        private void Nav_Standings_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new ClassementPage());
        private void Nav_News_Click(object sender, MouseButtonEventArgs e) { }
        private void Nav_Calendar_Click(object sender, MouseButtonEventArgs e)
            => OpenWindow(new CoursePage());
        private void Nav_Hub_Click(object sender, MouseButtonEventArgs e)
        {
            // Retourne à la fenêtre principale (MainWindow)
            Application.Current.MainWindow.Show();
            this.Close();
        }

        // ── FILTRES ───────────────────────────────────────────────
        // Ces boutons sont vides pour l'instant, à implémenter plus tard
        private void BtnAnnee_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnGP_Click(object sender, MouseButtonEventArgs e) { }
        private void BtnSession_Click(object sender, MouseButtonEventArgs e) { }
    }
}