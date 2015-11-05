using System;
using System.Windows.Forms;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        private readonly IGameMonitorFormViewModel viewModel;
        private readonly ILog logger;
//        private readonly Func<string, GameMonitorDisplay> gameMonitorDisplayFactory; 

        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger
        //    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GameMonitorForm()
        {
            InitializeComponent();

            //buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(viewModel);
        }

        public GameMonitorForm(IGameMonitorFormViewModel viewModel, Func<string, GameMonitorDisplay> gameMonitorDisplayFactory, Func<Type, ILog> loggerFactory) : this()
        {
            this.viewModel = viewModel;
            this.logger = loggerFactory(typeof(GameMonitorForm));

            //had to move this: does Autofac use this constructor?
            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(viewModel, gameMonitorDisplayFactory);
        }

        private void LoadMonitoringDisplay(IGameMonitorFormViewModel gameMonitorFormViewModel, Func<string, GameMonitorDisplay> gameMonitorDisplayFactory)
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK)
                return;

            if (gameMonitorFormViewModel.ShouldMonitor(chooseGameDialog.FileName))
            {
                var display = gameMonitorDisplayFactory(chooseGameDialog.FileName);
                mainPanel.Controls.Add(display);
            }
            else
            {
                MessageBox.Show(@"Program is already being Monitored.");
            }
        }
    }
}
