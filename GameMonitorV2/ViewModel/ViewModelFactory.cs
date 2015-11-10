using System;
using System.ComponentModel;
using log4net;

namespace GameMonitorV2.ViewModel
{
    class ViewModelFactory
    {
        public static GameMonitorDisplayViewModel CreateNewDisplayViewModel(ISynchronizeInvoke synchronizeInvoke, string fileNameAndPath, Func<Type, ILog> loggerFactory)
        {
            var gameMonitoryDisplayViewModel = new GameMonitorDisplayViewModel(synchronizeInvoke, fileNameAndPath, loggerFactory);
            return gameMonitoryDisplayViewModel;
        }

        public static GameMonitorFormViewModel CreateNewFormViewModel(Func<Type, ILog> loggerFactory)
        {
            var gameMonitorFormViewModel = new GameMonitorFormViewModel(loggerFactory);
            return gameMonitorFormViewModel;
        }
    }
}