using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductivityApp.UI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            SessionBox.Text = Properties.Settings.Default.SessionDuration.ToString("0.##");
            CooldownBox.Text = Properties.Settings.Default.CooldownDuration.ToString("0.##");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(SessionBox.Text, out double session))
            {
                Properties.Settings.Default.SessionDuration = session;
            }

            if (double.TryParse(CooldownBox.Text, out double cooldown))
            {
                Properties.Settings.Default.CooldownDuration = cooldown;
            }

            Properties.Settings.Default.Save();

            ShowToast("Settings Saved");
            Close();

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var main = (MainWindow)System.Windows.Application.Current.MainWindow;
                main.ReloadSettings();
            });
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); // simply exit without saving
        }


        private void ShowToast(string message)
        {
            var toast = new Window
            {
                Width = 250,
                Height = 60,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = System.Windows.Media.Brushes.Transparent,
                Topmost = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var border = new Border
            {
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Child = new TextBlock
                {
                    Text = "✔ " + message,
                    Foreground = System.Windows.Media.Brushes.LightGreen,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    FontSize = 14
                }
            };

            toast.Content = border;

            toast.Show();

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.2);

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                toast.Close();
            };

            timer.Start();
        }
    }
}
