using System.Linq;
using System.Windows.Forms;
using GameMonitorV2.ViewModel;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        private GameMonitorFormViewModel gameMonitorFormViewModel = new GameMonitorFormViewModel();

        public GameMonitorForm()
        {
            InitializeComponent();
            
            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay();
        }

        private void LoadMonitoringDisplay()
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK)
                return;

            //if (gameMonitorFormViewModel.CheckDuplicateMonitoring(chooseGameDialog.FileName)) return;
            if (CheckDuplicateMonitoring()) return;

            var display = new GameMonitorDisplay(chooseGameDialog.FileName);
            mainPanel.Controls.Add(display);

            //// Approach 2:
            //var filePicker = new FilePicker(gameMonitorFormViewModel.GetFilePickerViewModel());
            //var gameName = filePicker.Show();

            //if (gameMonitorFormViewModel.ShouldMonitor(gameName))
            //    ShowGameMonitorDisplay();
            //else

        }


        private bool CheckDuplicateMonitoring()
        {
            //if process is already monitored then display message
            if ((from Control panel in mainPanel.Controls
                where panel.GetType() == typeof (GameMonitorDisplay)
                select panel as GameMonitorDisplay).Any(item => item.FileName == chooseGameDialog.FileName))
            {
                MessageBox.Show(@"Program is already being Monitored.");
                return true;
            }
            return false;
        }
    }
}
