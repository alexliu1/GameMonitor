using System.Collections.Generic;
using System.Linq;

namespace GameMonitorV2.ViewModel
{
    public class GameMonitorFormViewModel
    {
        public List<string> MonitoredFiles { get; set; }

        public GameMonitorFormViewModel()
        {
            MonitoredFiles = new List<string>();
        }

        public bool CheckDuplicateMonitoring(string fileName)
        {
            if (MonitoredFiles.All(monitoredFile => monitoredFile != fileName)) return true;
            MonitoredFiles.Add(fileName);
            return false;
        }
    }
}