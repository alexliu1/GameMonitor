using System;
using log4net;

namespace GameMonitorV2.ViewModel
{
    public class GMFViewModelFactory
    {
        private readonly Func<Type, ILog> loggerFactory;

        public GMFViewModelFactory(Func<Type, ILog> loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public GameMonitorFormViewModel CreateNewFormViewModel()
        {
            var gameMonitorFormViewModel = new GameMonitorFormViewModel(loggerFactory);
            return gameMonitorFormViewModel;
        }
    }
}