using System;
using System.ComponentModel;
using System.Windows.Forms;
using Autofac;
using GameMonitorV2.Model;
using GameMonitorV2.View;
using GameMonitorV2.ViewModel;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GameMonitorV2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Autofac registration
            var builder = new ContainerBuilder();

            builder.Register<Func<Type, ILog>>(c => LogManager.GetLogger);
            
            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var gameDisplayFactory = c.Resolve<Func<string, GameMonitorDisplay>>();
                var viewModel = c.Resolve<IGameMonitorFormViewModel>();
                return new GameMonitorForm(viewModel, gameDisplayFactory, loggerFactory);

            }).As<GameMonitorForm>();

            builder.RegisterType<GameMonitorFormViewModel>().As<IGameMonitorFormViewModel>();

            builder.Register<Func<string, GameMonitorDisplay>>(context =>
            {
                //var context = c.Resolve<IComponentContext>();
                return fileName =>
                {
                    var loggerFactory = context.Resolve<Func<Type, ILog>>();
                    var logger = loggerFactory(typeof (GameMonitorDisplay));
                    var viewModelFactory = context.Resolve<Func<string, ISynchronizeInvoke, GameMonitorDisplayViewModel>>();
                    return new GameMonitorDisplay(viewModelFactory, fileName, logger);
                };
            });

            builder.Register<Func<string, ISynchronizeInvoke, GameMonitorDisplayViewModel>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return (fileName, synchronizeInvoke ) =>
                {
                    var loggerFactory = context.Resolve<Func<Type, ILog>>();
                    var logger = loggerFactory(typeof(GameMonitorDisplayViewModel));
                    var pollWatchFactory = context.Resolve<Func<string, PollWatcher>>();
                    return new GameMonitorDisplayViewModel(pollWatchFactory, synchronizeInvoke, fileName, logger);
                };
            });

            builder.Register<Func<string, PollWatcher>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return gameName =>
                {
                    var loggerFactory = context.Resolve<Func<Type, ILog>>();
                    var logger = loggerFactory(typeof (PollWatcher));
                    return new PollWatcher(gameName);
                };
            });
            
            var container = builder.Build();

            var gameMonitorForm = container.Resolve<GameMonitorForm>();
            Application.Run(gameMonitorForm);
        }
    }
}
