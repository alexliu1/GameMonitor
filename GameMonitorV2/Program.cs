using System;
using System.Windows.Forms;
using Autofac;
using GameMonitorV2.AutofacModule;
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
        /// 
        /// Program was developed in a topdown manner in the MVVM (Model-View-View-Model) pattern with focus
        /// on test-driven development environment. Autofac was used to handle dependency injection, and
        /// Log4Net was used to handle logging.
        /// 
        /// Program Flow:
        /// Main Application form: GameMonitorForm.cs, which creates a new GameMonitorDisplay object for each
        /// program monitored. Each GameMonitorDisplayViewModel creates a new PollWatcher object to monitor
        /// the selected game.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Autofac registration for Dependency Injection
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ViewModule());
            builder.RegisterModule(new ViewModelModule());
            builder.RegisterModule(new ModelModule());
            var container = builder.Build();

            var gameMonitorForm = container.Resolve<GameMonitorForm>();
            Application.Run(gameMonitorForm);
        }
    }
}
