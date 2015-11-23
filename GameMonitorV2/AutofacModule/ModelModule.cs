using System;
using Autofac;
using GameMonitorV2.Model;
using log4net;

namespace GameMonitorV2.AutofacModule
{
    public class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                return new PollWatcherFactory(loggerFactory);
            }).As<PollWatcherFactory>();
        }
    }
}