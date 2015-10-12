using System;
using System.Windows.Forms;
using GameMonitorV2.View;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ScratchProject
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

            //// Dependency injection
            //Register.RegisterWithContainer();
            //var gameMonitorForm = container.Get<IGameMonitorForm>();
            //Application.Run(gameMonitorForm);
            
            Application.Run(new GameMonitorForm());
        }
    }
}
