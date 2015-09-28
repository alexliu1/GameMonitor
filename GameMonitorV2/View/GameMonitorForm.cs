using System.Windows.Forms;

namespace GameMonitorV2.View
{
    public partial class GameMonitorForm : Form
    {
        public GameMonitorForm()
        {
            InitializeComponent();
            
            buttonLoadGame.Click += (sender, args) => LoadMonitoringDisplay();

        }

        private void LoadMonitoringDisplay()
        {
            if (chooseGameDialog.ShowDialog() != DialogResult.OK) 
                return;

            var display = new GameMonitorDisplay(chooseGameDialog.FileName);
            mainPanel.Controls.Add(display);
        }
    }
}
