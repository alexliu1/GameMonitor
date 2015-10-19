using log4net;

namespace GameMonitorV2.ViewModel
{
    public interface IGameMonitorFormViewModel
    {
        bool ShouldMonitor(string fileName);
    }
}