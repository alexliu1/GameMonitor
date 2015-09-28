using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace GameMonitorV2.Model
{
    public class PollWatcher
    {
        private const int interval = 1 * 1000;
        public string ProcessName { get; private set; }
        private int elapsedTime;

        public PollWatcher(string processName)
        {
            ProcessName = processName;

            var timer = new Timer { Interval = interval };
            timer.Elapsed += UpdatedElapsedTime;
            timer.Start();
        }

        public TimeSpan ElapsedTime
        {
            get { return TimeSpan.FromMilliseconds(elapsedTime); }
        }

        private void UpdatedElapsedTime(object sender, EventArgs e)
        {
            if (!ProcessExists())
                return;

            elapsedTime += interval;

            if (ElapsedTimeTick != null) ElapsedTimeTick();
        }

        private bool ProcessExists()
        {
            return Process.GetProcessesByName(ProcessName).Any();
        }

        public event Action ElapsedTimeTick;
    }
}
