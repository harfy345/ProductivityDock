using ProductivityApp.Core;
using ProductivityApp.UI;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProductivityApp
{
    public partial class MainWindow : Window
    {
        private enum TimerState
        {
            Stopped,
            Running,
            Paused
        }

        private TimerState _state = TimerState.Stopped;

        private UtilTimer _timer;
        private int _sessionDuration ;
        private int _cooldown ;
        private NotifyIcon _trayIcon;
        private bool _isExit;


        private readonly SolidColorBrush RunningBrush = new(Colors.LimeGreen);
        private readonly SolidColorBrush PausedBrush = new(Colors.Gold);
        private readonly SolidColorBrush StoppedBrush = new(Colors.Red);

        public MainWindow()
        {
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/app.ico"));

            _sessionDuration = MinutesToSeconds(Properties.Settings.Default.SessionDuration);
            _cooldown = MinutesToSeconds(Properties.Settings.Default.CooldownDuration);
            InitializeComponent();
            Timer_Label.Text = FormatTime(_sessionDuration);

            _timer = new UtilTimer();

            _timer.Tick += OnTick;
            _timer.Finished += OnTimerFinished;

            SetupTrayIcon();

            // Start hidden
            //Hide();

        }
        private int MinutesToSeconds(double minutes)
        {
            return (int)Math.Round(minutes * 60);
        }

        private string FormatTime(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return time.ToString(@"hh\:mm\:ss");
        }
        private void UpdateStatus(TimerState state)
        {
            _state = state;

            switch (state)
            {
                case TimerState.Running:
                    StatusIcon.Text = "▶";
                    StatusText.Text = "Running";
                    StatusIcon.Foreground = RunningBrush;
                    break;

                case TimerState.Paused:
                    StatusIcon.Text = "⏸";
                    StatusText.Text = "Paused";
                    StatusIcon.Foreground = PausedBrush;
                    break;

                case TimerState.Stopped:
                    StatusIcon.Text = "⏹";
                    StatusText.Text = "Stopped";
                    StatusIcon.Foreground = StoppedBrush;
                    break;
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            switch (_state)
            {
                case TimerState.Stopped:
                    StartBtn.IsEnabled = true;
                    PauseBtn.IsEnabled = false;
                    ResumeBtn.IsEnabled = false;
                    ResetBtn.IsEnabled = false;
                    break;

                case TimerState.Running:
                    StartBtn.IsEnabled = false;
                    PauseBtn.IsEnabled = true;
                    ResumeBtn.IsEnabled = false;
                    ResetBtn.IsEnabled = true;
                    break;

                case TimerState.Paused:
                    StartBtn.IsEnabled = false;
                    PauseBtn.IsEnabled = false;
                    ResumeBtn.IsEnabled = true;
                    ResetBtn.IsEnabled = true;
                    break;
            }
        }
        private void SetupTrayIcon()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = new Icon("./app.ico"), // add your icon file
                Visible = true,
                Text = "Productivity Dock"
            };

            _trayIcon.DoubleClick += (s, e) =>
            {
                ShowMainWindow();
            };

            var menu = new ContextMenuStrip();

            menu.Items.Add("Open", null, (s, e) =>
            {
                ShowMainWindow();
            });

            menu.Items.Add("Exit", null, (s, e) =>
            {
                _isExit = true;
                _trayIcon.Visible = false;
                System.Windows.Application.Current.Shutdown();
            });

            _trayIcon.ContextMenuStrip = menu;
        }
        private void ShowMainWindow()
        {
            Show();

            WindowState = WindowState.Normal;

            UpdateStatus(_state);
            Activate();
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start(_sessionDuration);
            UpdateStatus(TimerState.Running);

        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _timer.Reset(_sessionDuration);
            UpdateStatus(TimerState.Stopped);
        }

        private void OnTick(int seconds)
        {
            Dispatcher.Invoke(() =>
            {
                Timer_Label.Text = FormatTime(seconds);
            });
        }

        private void OnTimerFinished()
        {
            Dispatcher.Invoke(() =>
            {
                ShowBlockWindow();
            });
        }

        private void ShowBlockWindow()
        {
            var block = new BlockWindow(_cooldown);

            block.OnFinished += () =>
            {
                //_timer.Start(_sessionDuration);
            };

            block.ShowDialog();
        }

        private void startTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            Start_Click(sender, e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                Hide();
            }

            base.OnClosing(e);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow();

            settings.ShowDialog();

            // Reload values after settings change
            _sessionDuration = MinutesToSeconds(Properties.Settings.Default.SessionDuration);
            _cooldown = MinutesToSeconds(Properties.Settings.Default.CooldownDuration);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            UpdateStatus(TimerState.Paused);
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            _timer.Resume();
            UpdateStatus(TimerState.Running);
        }

        public void ReloadSettings()
        {
            _sessionDuration = MinutesToSeconds(Properties.Settings.Default.SessionDuration);
            _cooldown = MinutesToSeconds(Properties.Settings.Default.CooldownDuration);
        }

    }
}