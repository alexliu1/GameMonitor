using System;
using Autofac;
using GameMonitorV2.Model;
using GameMonitorV2.View;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2
{
    public class GameMonitorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<Func<Type, ILog>>(c => LogManager.GetLogger);

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var gameMonitorFormVMFactory = c.Resolve<GMFViewModelFactory>();
                var gameMonitorDisplayFactory = c.Resolve<GameMonitorDisplayFactory>();
                return new GameMonitorForm(gameMonitorFormVMFactory, gameMonitorDisplayFactory, loggerFactory);
            }).As<GameMonitorForm>();

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var gameMonitorDisplayViewModelFactory = c.Resolve<GMDViewModelFactory>();
                return new GameMonitorDisplayFactory(gameMonitorDisplayViewModelFactory, loggerFactory);
            }).As<GameMonitorDisplayFactory>();

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                return new GMFViewModelFactory(loggerFactory);
            }).As<GMFViewModelFactory>();

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var pollWatcherFactory = c.Resolve<PollWatcherFactory>();
                return new GMDViewModelFactory(pollWatcherFactory, loggerFactory);
            }).As<GMDViewModelFactory>();

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                return new PollWatcherFactory(loggerFactory);
            }).As<PollWatcherFactory>();
        }
    }
}