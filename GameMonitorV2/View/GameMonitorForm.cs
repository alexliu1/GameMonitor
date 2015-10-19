﻿using System;
using System.Windows.Forms;
using GameMonitorV2.ViewModel;
using log4net;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        private readonly IGameMonitorFormViewModel viewModel;
        private readonly ILog logger;

        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger
        //    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GameMonitorForm()
        {
            InitializeComponent();

            //buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(viewModel);
        }

       // public GameMonitorForm(IGameMonitorFormViewModel viewModel, Func<string, ILog, GameMonitorDisplay> gameMonitorDisplayFunc, Func<Type, ILog> loggerFactory) : this()
        public GameMonitorForm(IGameMonitorFormViewModel viewModel, Func<Type, ILog> loggerFactory) : this()
        {
            this.viewModel = viewModel;
            this.logger = loggerFactory(typeof(GameMonitorForm));

            //had to move this: does Autofac use this constructor?
            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(viewModel);
        }

        private void LoadMonitoringDisplay(IGameMonitorFormViewModel gameMonitorFormViewModel)
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK)
                return;

            if (gameMonitorFormViewModel.ShouldMonitor(chooseGameDialog.FileName))
            {
                // var display = gameMonitorDisplayFactory.Create(chooseGameDialog.FileName, loggerFactory); // factory
                // var display = gameMonitorDisplayFactory(chooseGameDialog.FileName, loggerFactory); // func   
                // var display = gameMonitorDisplayFactory(chooseGameDialog.FileName); // func  - better 
                //var display = new GameMonitorDisplay(chooseGameDialog.FileName, logger);
                mainPanel.Controls.Add(display);
            }
            else
            {
                MessageBox.Show(@"Program is already being Monitored.");
            }
            //// Approach 2:
            //var filePicker = new FilePicker(gameMonitorFormViewModel.GetFilePickerViewModel());
            //var gameName = filePicker.Show();

            //if (gameMonitorFormViewModel.ShouldMonitor(gameName))
            //    ShowGameMonitorDisplay();
            //else
        }
    }
}
