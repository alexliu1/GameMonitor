using System.Drawing;
using System.Linq.Expressions;
using System.Media;
using System.Windows.Forms;
using GameMonitorV2.ViewModel;

namespace GameMonitorV2.View
{
    public partial class GameMonitorDisplay : UserControl
    {
        private static bool soundPlayed ;

        public GameMonitorDisplay(string fileNameAndPath)
        {
            InitializeComponent();

            var gameMonitorDisplayViewModel = new GameMonitorDisplayViewModel(this, fileNameAndPath);

            BindLabelToProperties(gameMonitorDisplayViewModel);
            SubscribeToTimeExpirationEvent(gameMonitorDisplayViewModel);
        }

        private void BindLabelToProperties(GameMonitorDisplayViewModel gameMonitorDisplayViewModel)
        {
            var gameNameBinding = new Binding("Text", gameMonitorDisplayViewModel, "GameName");
            labelGame.DataBindings.Add(gameNameBinding);
            var runningTimeBinding = new Binding("Text", gameMonitorDisplayViewModel, "ElapsedTime");
            labelTime.DataBindings.Add(runningTimeBinding);
        }

        private void SubscribeToTimeExpirationEvent(GameMonitorDisplayViewModel gameMonitorDisplayViewModel)
        {
            gameMonitorDisplayViewModel.TimeExpired += () =>
            {
                OnButtonPropertyChange();
                OnLabelColorChange();
                PlayTimeExpiredWarningSound();
            };
            buttonClose.Click += (sender, args) =>
            {
                gameMonitorDisplayViewModel.CloseProgram();
                ResetAttributes();
            };
        }

        private void ResetAttributes()
        {
            soundPlayed = false;
            labelTime.ForeColor = DefaultForeColor;
            buttonClose.Enabled = false;
        }

        private static void PlayTimeExpiredWarningSound()
        {
            if (soundPlayed) return;
            SystemSounds.Exclamation.Play();
            soundPlayed = true;
        }

        private void OnLabelColorChange()
        {
            if (labelTime.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(OnLabelColorChange), null);
            }
            else
            {
                labelTime.ForeColor = Color.Red;
            }
        }

        private void OnButtonPropertyChange()
        {
            if (buttonClose.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(OnButtonPropertyChange), null);
            }
            else
            {
                buttonClose.Enabled = true;
            }
        }
    }
}
