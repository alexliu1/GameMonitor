using System.Windows.Forms;
using GameMonitorV2.ViewModel;

namespace GameMonitorV2.View
{
    public partial class GameMonitorDisplay : UserControl
    {
        public GameMonitorDisplay(string fileNameAndPath)
        {
            InitializeComponent();

            var gameMonitorDisplayViewModel = new GameMonitorDisplayViewModel(this, fileNameAndPath);

            BindLabelToProperties(gameMonitorDisplayViewModel);
        }

        private void BindLabelToProperties(GameMonitorDisplayViewModel gameMonitorDisplayViewModel)
        {
            var gameNameBinding = new Binding("Text", gameMonitorDisplayViewModel, "GameName");
            labelGame.DataBindings.Add(gameNameBinding);
            var runningTimeBinding = new Binding("Text", gameMonitorDisplayViewModel, "ElapsedTime");
            labelTime.DataBindings.Add(runningTimeBinding);
        }
    }
}
