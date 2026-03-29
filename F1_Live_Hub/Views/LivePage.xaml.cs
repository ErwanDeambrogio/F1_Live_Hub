using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace F1_Live_Hub.Views
{
    public partial class LivePage : Window
    {
        public LivePage()
        {
            InitializeComponent();
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
                EnvoyerMessage();
        }

        private void BtnEnvoyer_Click(object sender, MouseButtonEventArgs e)
        {
            EnvoyerMessage();
        }

        private void EnvoyerMessage()
        {
            var texte = TxtMessage.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(texte)) return;

            // Bulle utilisateur
            AjouterMessage(texte, estUtilisateur: true);
            TxtMessage.Text = "";

            // Réponse IA placeholder (tu remplaceras par ton IA)
            AjouterMessageIA("Je traite ta question sur : \"" + texte + "\". Cette fonctionnalité IA sera bientôt disponible !");
        }

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

            var txt = new TextBlock
            {
                Text = texte,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 18
            };

            bubble.Child = txt;
            container.Children.Add(bubble);

            var time = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55)),
                FontSize = 9,
                Margin = new Thickness(estUtilisateur ? 0 : 4, 4, estUtilisateur ? 4 : 0, 0),
                HorizontalAlignment = estUtilisateur
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left
            };
            container.Children.Add(time);

            MessagesPanel.Children.Add(container);
            ScrollMessages.ScrollToBottom();
        }

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
            var header = new TextBlock
            {
                Text = "🏎 Assistant F1",
                Foreground = new SolidColorBrush(Color.FromRgb(0xE8, 0x00, 0x2D)),
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 4)
            };
            var txt = new TextBlock
            {
                Text = texte,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 18
            };

            inner.Children.Add(header);
            inner.Children.Add(txt);
            bubble.Child = inner;
            container.Children.Add(bubble);

            var time = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55)),
                FontSize = 9,
                Margin = new Thickness(4, 4, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            container.Children.Add(time);

            MessagesPanel.Children.Add(container);
            ScrollMessages.ScrollToBottom();
        }
    }
}