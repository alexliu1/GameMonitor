using System;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2.View
{
    public class GameMonitorDisplayFactory
    {
        private readonly Func<Type, ILog> loggerFactory;
        private readonly GameModelDisplayViewModelFactory gameMonitorDisplayViewModelFactory;

        public GameMonitorDisplayFactory(GameModelDisplayViewModelFactory gameMonitorDisplayViewModelFactory, Func<Type, ILog> loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.gameMonitorDisplayViewModelFactory = gameMonitorDisplayViewModelFactory;
        }

        public GameMonitorDisplay CreateNewDisplay(string fileNameAndPath)
        {
            var gameMonitorDisplay = new GameMonitorDisplay(fileNameAndPath, gameMonitorDisplayViewModelFactory, loggerFactory);
            return gameMonitorDisplay;
        }
    }
}