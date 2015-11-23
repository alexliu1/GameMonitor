using System;
using System.Windows.Forms;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        private readonly ILog logger;
        
        public GameMonitorForm()
        {
            InitializeComponent();
        }

        public GameMonitorForm(GameModelFormViewModelFactory gMFVMFactory, GameMonitorDisplayFactory gameMonitorDisplayFactory, Func<Type, ILog> loggerFactory) : this()
        {
            logger = loggerFactory(typeof(GameMonitorForm));
            var viewModel = gMFVMFactory.CreateNewFormViewModel();

            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(viewModel, gameMonitorDisplayFactory);
        }

        private void LoadMonitoringDisplay(GameMonitorFormViewModel gameMonitorFormViewModel, GameMonitorDisplayFactory gameMonitorDisplayFactory)
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK)
            {
                logger.Debug("Browse File Dialog did not return Okay.");
                return;
            }

            if (gameMonitorFormViewModel.ShouldMonitor(chooseGameDialog.FileName))
            {
                logger.Debug("Adding a new Game Monitoring Display.");
                var display = gameMonitorDisplayFactory.CreateNewDisplay(chooseGameDialog.FileName);
                mainPanel.Controls.Add(display);
            }
            else
            {
                logger.Debug("Duplicate file name chosen.");
                MessageBox.Show(@"Program is already being Monitored.");
            }
        }
    }
}
