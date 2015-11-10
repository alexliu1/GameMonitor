using System;
using System.Windows.Forms;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        private readonly ILog logger;

        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger
        //    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GameMonitorForm()
        {
            InitializeComponent();
        }

        public GameMonitorForm(Func<Type, ILog> loggerFactory) : this()
        {
            logger = loggerFactory(typeof(GameMonitorForm));
            var viewModel = ViewModelFactory.CreateNewFormViewModel(loggerFactory);

            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(viewModel, loggerFactory);
        }

        private void LoadMonitoringDisplay(GameMonitorFormViewModel gameMonitorFormViewModel, Func<Type, ILog> loggerFactory)
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK)
            {
                logger.Debug("Browse File Dialog did not return Okay.");
                return;
            }

            if (gameMonitorFormViewModel.ShouldMonitor(chooseGameDialog.FileName))
            {
                logger.Debug("Adding a new Game Monitoring Display.");
                var display = ViewFactory.CreateNewDisplay(chooseGameDialog.FileName, loggerFactory);
                mainPanel.Controls.Add(display);
            }
            else
            {
                logger.Debug("duplicate file name chosen.");
                MessageBox.Show(@"Program is already being Monitored.");
            }
        }
    }
}
