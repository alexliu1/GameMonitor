using System.Windows.Forms;

namespace GameMonitorV2.View
{
    public partial class GameMonitorDisplay : UserControl
    {
        public string GameName { get; set; }
        public string RunningTime { get; set; }

        public GameMonitorDisplay()
        {
            InitializeComponent();

            BindLabelToProperties();
        }

        private void BindLabelToProperties()
        {
            var gameNameBinding = new Binding("Text", this, "GameName");
            labelGame.DataBindings.Add(gameNameBinding);
            var runningTimeBinding = new Binding("Text", this, "RunningTime");
            labelTime.DataBindings.Add(runningTimeBinding);
        }
    }
}
