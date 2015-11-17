using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using log4net;

namespace GameMonitorV2.Model
{
    public class PollWatcher
    {
        private const int Interval = 1 * 1000;
        public string ProcessName { get; private set; }
        private int elapsedTime;
        private readonly ILog logger;

        public PollWatcher(string processName, Func<Type, ILog> loggerFactory)
        {
            logger = loggerFactory(typeof (PollWatcher));
            ProcessName = processName;

            var timer = new Timer { Interval = Interval };
            logger.Info(string.Format("Timer is started with an interval of [{0}ms]", Interval));
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
            {
                logger.Debug("Watched Program has ended.");
                return;
            }

            elapsedTime += Interval;
            logger.Debug(string.Format("Program elapsed Time = {0}", elapsedTime));

            if (ElapsedTimeTick != null) ElapsedTimeTick();
        }

        private bool ProcessExists()
        {
            return Process.GetProcessesByName(ProcessName).Any();
        }

        public event Action ElapsedTimeTick;
    }
}
