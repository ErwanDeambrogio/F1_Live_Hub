using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace F1_Live_Hub.Views
{
    public partial class LivePage : Window
    {
       
        private const string TOKEN = "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";

        private readonly HttpClient _client = new HttpClient();

        // Historique de la conversation pour que le bot se souvienne du contexte
        private List<object> _historique = new List<object>();

        public LivePage()
        {
            InitializeComponent();

            // Instructions strictes : le bot parle UNIQUEMENT de F1
            _historique.Add(new
            {
                role = "system",
                content = "Tu es un assistant expert en Formule 1. Tu réponds UNIQUEMENT aux questions sur la F1 : pilotes, écuries, courses, classements, histoire, règles, circuits. Si la question ne concerne pas la F1, réponds poliment que tu ne peux parler que de F1. Réponds en français, de manière courte et claire."
            });
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }

        private void BtnFermer_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _ = EnvoyerMessage();
        }

        private void BtnEnvoyer_Click(object sender, MouseButtonEventArgs e)
        {
            _ = EnvoyerMessage();
        }

        private async Task EnvoyerMessage()
        {
            var texte = TxtMessage.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(texte)) return;

            // Affiche le message de l'utilisateur
            AjouterMessage(texte, estUtilisateur: true);
            TxtMessage.Text = "";

            // Affiche "En train d'écrire..."
            var typing = AjouterTyping();

            // Ajoute le message à l'historique
            _historique.Add(new { role = "user", content = texte });

            try
            {
                // Appel API GitHub Models
                var body = new
                {
                    model = "gpt-4o",
                    messages = _historique,
                    max_tokens = 500
                };

                var request = new HttpRequestMessage(HttpMethod.Post,
                    "https://models.inference.ai.azure.com/chat/completions");
                request.Headers.Add("Authorization", "Bearer " + TOKEN);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json");

                var response = await _client.SendAsync(request);
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                var reponse = json["choices"]?[0]?["message"]?["content"]?.ToString()
                              ?? "Je n'ai pas pu répondre.";

                // Ajoute la réponse à l'historique
                _historique.Add(new { role = "assistant", content = reponse });

                // Supprime "En train d'écrire..." et affiche la réponse
                MessagesPanel.Children.Remove(typing);
                AjouterMessageIA(reponse);
            }
            catch (Exception ex)
            {
                MessagesPanel.Children.Remove(typing);
                AjouterMessageIA("Erreur : " + ex.Message);
            }
        }

        // Bulle "En train d'écrire..."
        private Border AjouterTyping()
        {
            var bubble = new Border
            {
                CornerRadius = new CornerRadius(4, 12, 12, 12),
                Padding = new Thickness(12, 10, 12, 10),
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                Margin = new Thickness(0, 0, 0, 12),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            bubble.Child = new TextBlock
            {
                Text = "✍️ En train d'écrire...",
                Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88)),
                FontSize = 12
            };
            MessagesPanel.Children.Add(bubble);
            ScrollMessages.ScrollToBottom();
            return bubble;
        }

        // Bulle message utilisateur ou IA
        private void AjouterMessage(string texte, bool estUtilisateur)
        {
            var container = new StackPanel
            {
                Margin = new Thickness(0, 0, 0, 12),
                HorizontalAlignment = estUtilisateur
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left,
                MaxWidth = 280
            };

            var bubble = new Border
            {
                CornerRadius = estUtilisateur
                    ? new CornerRadius(12, 4, 12, 12)
                    : new CornerRadius(4, 12, 12, 12),
                Padding = new Thickness(12, 10, 12, 10),
                Background = estUtilisateur
                    ? new SolidColorBrush(Color.FromRgb(0xE8, 0x00, 0x2D))
                    : new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A))
            };

            bubble.Child = new TextBlock
            {
                Text = texte,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 18
            };

            container.Children.Add(bubble);
            container.Children.Add(new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55)),
                FontSize = 9,
                Margin = new Thickness(estUtilisateur ? 0 : 4, 4, estUtilisateur ? 4 : 0, 0),
                HorizontalAlignment = estUtilisateur
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left
            });

            MessagesPanel.Children.Add(container);
            ScrollMessages.ScrollToBottom();
        }

        // Bulle réponse IA avec label "Assistant F1"
        private void AjouterMessageIA(string texte)
        {
            var container = new StackPanel
            {
                Margin = new Thickness(0, 0, 0, 12),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 280
            };

            var bubble = new Border
            {
                CornerRadius = new CornerRadius(4, 12, 12, 12),
                Padding = new Thickness(12, 10, 12, 10),
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A))
            };

            var inner = new StackPanel();
            inner.Children.Add(new TextBlock
            {
                Text = "🏎 Assistant F1",
                Foreground = new SolidColorBrush(Color.FromRgb(0xE8, 0x00, 0x2D)),
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 4)
            });
            inner.Children.Add(new TextBlock
            {
                Text = texte,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 18
            });

            bubble.Child = inner;
            container.Children.Add(bubble);
            container.Children.Add(new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55)),
                FontSize = 9,
                Margin = new Thickness(4, 4, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            });

            MessagesPanel.Children.Add(container);
            ScrollMessages.ScrollToBottom();
        }
    }
}