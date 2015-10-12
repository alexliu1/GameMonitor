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

            builder.Register<Func<Type, ILog>>(c => LogManager.GetLogger);

            builder.RegisterType<GameMonitorFormViewModel>().As<IGameMonitorFormViewModel>();
            builder.RegisterType<GameMonitorForm>();
            
            var container = builder.Build();

            //// Resolve main form
            var gameMonitorForm = container.Resolve<GameMonitorForm>();
            Application.Run(gameMonitorForm);
            
            //Application.Run(new GameMonitorForm());
        }
    }
}
