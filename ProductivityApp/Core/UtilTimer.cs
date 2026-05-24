using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace ProductivityApp.Core
{
    class UtilTimer
    {
        private System.Timers.Timer _timer;
        private int _remainingSeconds;
        private int _initialSeconds;

        public bool IsRunning { get; private set; }

        public event Action<int> Tick;
        public event Action Finished;

        public UtilTimer()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTick;
        }

        // START = reset
        public void Start(int seconds)
        {
            _initialSeconds = seconds;
            _remainingSeconds = seconds;

            IsRunning = true;
            _timer.Start();

            Tick?.Invoke(_remainingSeconds);
        }

        // PAUSE = stop but keep time
        public void Stop()
        {
            IsRunning = false;
            _timer.Stop();
        }

        // RESUME = continue from remaining
        public void Resume()
        {
            if (_remainingSeconds <= 0)
                _remainingSeconds = _initialSeconds;

            IsRunning = true;
            _timer.Start();
        }

        public void Reset(int seconds)
        {
            Stop();
            _remainingSeconds = seconds;
            _initialSeconds = seconds;

            Tick?.Invoke(_remainingSeconds);
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            if (!IsRunning) return;

            _remainingSeconds--;

            Tick?.Invoke(_remainingSeconds);

            if (_remainingSeconds <= 0)
            {
                Stop();
                Finished?.Invoke();
            }
        }
    }
}
