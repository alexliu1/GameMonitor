using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace GameMonitorV2.ViewModel
{
    public class GameMonitorFormViewModel : IGameMonitorFormViewModel
    {
        private ILog logger;

        public GameMonitorFormViewModel(Func<Type, ILog> loggerFactory)
        {
            logger = loggerFactory(typeof(GameMonitorFormViewModel));

            MonitoredFiles = new List<string>();
        }

        private List<string> MonitoredFiles { get; set; }

        public bool ShouldMonitor(string fileName)
        {
            if (MonitoredFiles.Any(monitoredFile => monitoredFile == fileName))
            {
                logger.Debug(string.Format("Process [{0}] is already being monitored", fileName));
                return false;
            }
            
            MonitoredFiles.Add(fileName);
            return true;
        }
    }
}