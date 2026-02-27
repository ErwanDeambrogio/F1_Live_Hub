using F1_Live_Hub.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace F1_Live_Hub
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppFrame.Navigate(new AccueilPage());
            Loaded += async (s, e) => await RunSplashAnimation();
        }

        private async Task RunSplashAnimation()
        {
            var easeOut = new CubicEase { EasingMode = EasingMode.EaseOut };
            var easeInOut = new CubicEase { EasingMode = EasingMode.EaseInOut };

            // 1. Grille fond
            await Anim(SplashGrid2, UIElement.OpacityProperty, 0, 1, 600, easeOut);

            // 2. Lignes rouges
            await Task.WhenAll(
                Anim(SplashLineTop, FrameworkElement.WidthProperty, 0, 300, 500, easeOut),
                Anim(SplashLineBottom, FrameworkElement.WidthProperty, 0, 300, 500, easeOut)
            );

            // 3. Glow centre
            await Anim(SplashGlow, UIElement.OpacityProperty, 0, 1, 400, easeOut);

            // 4. Logo scale in
            await Task.WhenAll(
                Anim(SplashLogoGroup, UIElement.OpacityProperty, 0.0, 1.0, 700, easeOut),
                Anim(SplashLogoScale, ScaleTransform.ScaleXProperty, 0.6, 1.0, 700, easeOut),
                Anim(SplashLogoScale, ScaleTransform.ScaleYProperty, 0.6, 1.0, 700, easeOut)
            );

            // 5. Glow pulse badge
            await Anim(SplashLogoGlow, DropShadowEffect.BlurRadiusProperty, 0, 35, 400, easeOut);
            await Anim(SplashLogoGlow, DropShadowEffect.BlurRadiusProperty, 35, 18, 400, easeInOut);

            // 6. HUB + sous-ligne
            await Task.WhenAll(
                Anim(SplashHubText, UIElement.OpacityProperty, 0, 1, 400, easeOut),
                Anim(SplashSubLine, FrameworkElement.WidthProperty, 0, 140, 500, easeOut)
            );

            // 7. Tagline
            await Anim(SplashTagline, UIElement.OpacityProperty, 0, 1, 400, easeOut);

            await Task.Delay(200);

            // 8. Barre de chargement
            await Anim(SplashLoadText, UIElement.OpacityProperty, 0, 1, 300, easeOut);
            await Anim(SplashLoadBar, FrameworkElement.WidthProperty, 0, 200, 1200, easeInOut);

            await Task.Delay(200);

            // 9. Pulse final logo
            await Task.WhenAll(
                Anim(SplashLogoScale, ScaleTransform.ScaleXProperty, 1.0, 1.08, 180, easeOut),
                Anim(SplashLogoScale, ScaleTransform.ScaleYProperty, 1.0, 1.08, 180, easeOut)
            );
            await Task.WhenAll(
                Anim(SplashLogoScale, ScaleTransform.ScaleXProperty, 1.08, 1.0, 180, easeInOut),
                Anim(SplashLogoScale, ScaleTransform.ScaleYProperty, 1.08, 1.0, 180, easeInOut)
            );

            await Task.Delay(150);

            // 10. Flash blanc
            await Anim(SplashFlash, UIElement.OpacityProperty, 0, 1, 250, easeOut);

            // Transition vers AccueilPage
            SplashGrid.Visibility = Visibility.Collapsed;
            await Anim(AppFrame, UIElement.OpacityProperty, 0, 1, 400, easeOut);
        }

        private Task Anim(
            DependencyObject target,
            DependencyProperty property,
            double from, double to,
            double ms,
            IEasingFunction easing = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            var a = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(ms)),
                EasingFunction = easing,
                FillBehavior = FillBehavior.HoldEnd
            };
            a.Completed += (s, e) => tcs.TrySetResult(true);

            if (target is Animatable animatable)
                animatable.BeginAnimation(property, a);
            else if (target is UIElement ui)
                ui.BeginAnimation(property, a);

            return tcs.Task;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseButton_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}