using System;
using System.Windows.Forms;
using Autofac;
using GameMonitorV2.View;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GameMonitorV2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// User click "Browse" to find a game or program to monitor.
        /// Application will monitor the elapsed run time of the selected game or program.
        /// Once the game or program has exceeded the pre-determined limit (3 hours), it will give a
        /// warning and allow the user to shut down the game or program.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Autofac registration for Dependency Injection
            var builder = new ContainerBuilder();
            builder.RegisterModule(new GameMonitorModule());
            var container = builder.Build();

            var gameMonitorForm = container.Resolve<GameMonitorForm>();
            Application.Run(gameMonitorForm);
        }
    }
}
