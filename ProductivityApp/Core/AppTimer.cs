using System;
using System.Timers;

namespace ProductivityApp.Core
{
    public class AppTimer
    {
        private System.Timers.Timer _timer;
        private int _remainingSeconds;

        public bool IsRunning { get; private set; }

        public event Action<int> TickChanged;
        public event Action Finished;

        public AppTimer(int seconds)
        {
            _remainingSeconds = seconds;

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += Tick;
        }

        public void Start(int seconds)
        {
            _remainingSeconds = seconds;
            IsRunning = true;
            _timer.Start();
        }

        public void Stop()
        {
            IsRunning = false;
            _timer.Stop();
        }

        public void Reset(int seconds)
        {
            Stop();
            _remainingSeconds = seconds;
            TickChanged?.Invoke(_remainingSeconds);
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            if (!IsRunning) return;

            _remainingSeconds--;

            TickChanged?.Invoke(_remainingSeconds);

            if (_remainingSeconds <= 0)
            {
                Stop();
                Finished?.Invoke();
            }
        }
    }
}
