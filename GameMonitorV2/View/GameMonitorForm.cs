using System.Windows.Forms;
using GameMonitorV2.ViewModel;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GameMonitorForm()
        {
            InitializeComponent();

            GameMonitorFormViewModel gameMonitorFormViewModel = new GameMonitorFormViewModel(log);

            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(gameMonitorFormViewModel);
        }

        private void LoadMonitoringDisplay(GameMonitorFormViewModel gameMonitorFormViewModel)
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK)
                return;

            if (gameMonitorFormViewModel.ShouldMonitor(chooseGameDialog.FileName))
            {
                var display = new GameMonitorDisplay(chooseGameDialog.FileName, log);
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
