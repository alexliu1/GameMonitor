using System;
using Autofac;
using GameMonitorV2.Model;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2.AutofacModule
{
    public class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                return new GameModelFormViewModelFactory(loggerFactory);
            }).As<GameModelFormViewModelFactory>();

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var pollWatcherFactory = c.Resolve<PollWatcherFactory>();
                return new GameModelDisplayViewModelFactory(pollWatcherFactory, loggerFactory);
            }).As<GameModelDisplayViewModelFactory>();
        }
    }
}