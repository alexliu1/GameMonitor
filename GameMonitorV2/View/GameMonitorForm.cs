using System.IO;
using System.Windows.Forms;
using GameMonitorV2.ViewModel;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        public GameMonitorForm()
        {
            InitializeComponent();

            var gameMonitorFormViewModel = new GameMonitorFormViewModel(this);

            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay(gameMonitorFormViewModel);

        }

        private void LoadMonitoringDisplay(GameMonitorFormViewModel gameMonitorFormViewModel)
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK) 
                return;

            //gameMonitorFormViewModel.GameName = Path.GetFileNameWithoutExtension(chooseGameDialog.FileName);
            //gameMonitorFormViewModel.LoadGameToBeMonitored();

            var display = new GameMonitorDisplay(chooseGameDialog.FileName);
            mainPanel.Controls.Add(display);
        }
    }
}
