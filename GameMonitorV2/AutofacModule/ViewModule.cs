using System;
using Autofac;
using GameMonitorV2.View;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2.AutofacModule
{
    public class ViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<Func<Type, ILog>>(c => LogManager.GetLogger);

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var gameMonitorFormViewModelFactory = c.Resolve<GameModelFormViewModelFactory>();
                var gameMonitorDisplayFactory = c.Resolve<GameMonitorDisplayFactory>();
                return new GameMonitorForm(gameMonitorFormViewModelFactory, gameMonitorDisplayFactory, loggerFactory);
            }).As<GameMonitorForm>();

            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var gameMonitorDisplayViewModelFactory = c.Resolve<GameModelDisplayViewModelFactory>();
                return new GameMonitorDisplayFactory(gameMonitorDisplayViewModelFactory, loggerFactory);
            }).As<GameMonitorDisplayFactory>();
        }
    }
}