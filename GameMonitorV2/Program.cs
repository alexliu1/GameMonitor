using System;
using System.Windows.Forms;
using Autofac;
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

            //builder.RegisterType<ILog>().As<ILog>();
            builder.Register<Func<Type, ILog>>(c => LogManager.GetLogger);
            
            builder.Register(c =>
            {
                var loggerFactory = c.Resolve<Func<Type, ILog>>();
                var viewModel = c.Resolve<IGameMonitorFormViewModel>();
                return new GameMonitorForm(viewModel, loggerFactory);

            }).As<GameMonitorForm>();
            //builder.RegisterType<GameMonitorForm>();

            builder.Register<Func<string, GameMonitorDisplay>>(c =>
            {
                return fileName =>
                {
                    var loggerFactory = c.Resolve<Func<Type, ILog>>();
                    var logger = loggerFactory(typeof (GameMonitorDisplay));
                    return new GameMonitorDisplay(fileName, logger);
                };
            });
            builder.RegisterType<GameMonitorDisplay>();

            builder.RegisterType<GameMonitorFormViewModel>().As<IGameMonitorFormViewModel>();
            
            var container = builder.Build();

            //// Resolve main form
            using (var scope = container.BeginLifetimeScope())
            {
                var display1 = scope.Resolve<GameMonitorDisplay>();
                var display2 = scope.Resolve<GameMonitorDisplay>();
                //var gameMonitorFormViewModel = scope.Resolve<GameMonitorFormViewModel>();
                //var loggerFactory = scope.Resolve<Func<Type, ILog>>();
                //var gameMonitorForm = container.Resolve<GameMonitorForm>(gameMonitorFormViewModel, loggerFactory);
            var gameMonitorForm = container.Resolve<GameMonitorForm>();
            Application.Run(gameMonitorForm);
            }

            //Application.Run(new GameMonitorForm());
        }
    }
}
