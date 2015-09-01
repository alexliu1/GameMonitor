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
            
            var display = new GameMonitorDisplay();
            mainPanel.Controls.Add(display);

        }
    }
}
