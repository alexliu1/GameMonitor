using System;
using log4net;

namespace GameMonitorV2.Model
{
    public class PollWatcherFactory
    {
        private readonly Func<Type, ILog> loggerFactory;

        public PollWatcherFactory(Func<Type, ILog> loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public PollWatcher CreateNewPollWatcher(string gameName)
        {
            var pollWatcher = new PollWatcher(gameName, loggerFactory);
            return pollWatcher;
        }
    }
}