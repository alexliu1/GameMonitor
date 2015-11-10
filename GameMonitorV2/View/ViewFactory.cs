using System;
using log4net;

namespace GameMonitorV2.View
{
    class ViewFactory
    {
        public static GameMonitorDisplay CreateNewDisplay(string fileNameAndPath, Func<Type, ILog> loggerFactory)
        {
            var gameMonitorDisplay = new GameMonitorDisplay(fileNameAndPath, loggerFactory);
            return gameMonitorDisplay;
        }
    }
}