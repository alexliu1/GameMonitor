using System;
using log4net;

namespace GameMonitorV2.Model
{
    class ModelFactory
    {
        public static PollWatcher CreateNewPollWatcher(string gameName, Func<Type, ILog> loggerFactory)
        {
            var pollWatcher = new PollWatcher(gameName, loggerFactory);
            return pollWatcher;
        }
    }
}