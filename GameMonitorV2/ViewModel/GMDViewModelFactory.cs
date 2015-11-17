using System;
using System.ComponentModel;
using GameMonitorV2.Model;
using log4net;

namespace GameMonitorV2.ViewModel
{
    public class GMDViewModelFactory
    {
        private readonly Func<Type, ILog> loggerFactory;
        private readonly PollWatcherFactory pollWatcherFactory;

        public GMDViewModelFactory(PollWatcherFactory pollWatcherFactory, Func<Type, ILog> loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.pollWatcherFactory = pollWatcherFactory;
        }

        public GameMonitorDisplayViewModel CreateNewDisplayViewModel(ISynchronizeInvoke synchronizeInvoke, string fileNameAndPath)
        {
            var gameMonitoryDisplayViewModel = new GameMonitorDisplayViewModel(synchronizeInvoke, fileNameAndPath, pollWatcherFactory, loggerFactory);
            return gameMonitoryDisplayViewModel;
        }
    }
}