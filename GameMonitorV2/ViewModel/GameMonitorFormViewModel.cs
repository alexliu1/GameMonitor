using System.Collections.Generic;
using System.Linq;
using log4net;

namespace GameMonitorV2.ViewModel
{
    public class GameMonitorFormViewModel
    {
        public List<string> MonitoredFiles { get; set; }
        private ILog log;

        public GameMonitorFormViewModel(ILog log)
        {
            this.log = log;

            MonitoredFiles = new List<string>();
        }

        public bool ShouldMonitor(string fileName)
        {
            if (MonitoredFiles.Any(monitoredFile => monitoredFile == fileName))
            {
                return false;
            }
            else
            {
                MonitoredFiles.Add(fileName);
                return true;
            }
        }
    }
}