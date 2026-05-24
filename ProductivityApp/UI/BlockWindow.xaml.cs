
using ProductivityApp.Core;
using System.Windows;

using System.Windows.Threading;

namespace ProductivityApp.UI
{
    /// <summary>
    /// Interaction logic for BlockWindow.xaml
    /// </summary>
    public partial class BlockWindow : Window
    {
        private UtilTimer _timer;

        public event Action OnFinished;

        public BlockWindow(int cooldownSeconds)
        {
            InitializeComponent();

            Topmost = true;
            //WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;

            _timer = new UtilTimer();

            _timer.Tick += UpdateUI;
            _timer.Finished += Finish;

            Dispatcher.Invoke(() =>
            {
                TimerText.Text = cooldownSeconds.ToString();
            });
            _timer.Start(cooldownSeconds);
        }

        private void UpdateUI(int seconds)
        {
            Dispatcher.Invoke(() =>
            {
                TimerText.Text = seconds.ToString();
            });
        }

        private void Override_Click(object sender, RoutedEventArgs e)
        {
            Finish();
        }

        private void Finish()
        {
            _timer.Stop();
            Dispatcher.Invoke(() =>
            {
                Close();
            });
            OnFinished?.Invoke();
        }
    }
}
