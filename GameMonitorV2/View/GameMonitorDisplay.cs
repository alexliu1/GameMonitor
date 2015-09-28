using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace GameMonitorV2.View
{
    public partial class GameMonitorDisplay : UserControl
    {
        private bool soundPlayed ;

        public GameMonitorDisplay(string fileNameAndPath)
        {
            InitializeComponent();

            var gameMonitorDisplayViewModel = new GameMonitorDisplayViewModel(this, fileNameAndPath);

            BindLabelToProperties(gameMonitorDisplayViewModel);
            SubscribeToTimeExpired(gameMonitorDisplayViewModel);
            SubscribeToButtonCloseClick(gameMonitorDisplayViewModel);
        }

        private void SubscribeToTimeExpired(GameMonitorDisplayViewModel gameMonitorDisplayViewModel)
        {
            gameMonitorDisplayViewModel.TimeExpired += HandleTimeExpired;
        }

        private void SubscribeToButtonCloseClick(GameMonitorDisplayViewModel gameMonitorDisplayViewModel)
        {
            buttonClose.Click += (sender, args) =>
            {
                gameMonitorDisplayViewModel.CloseProgram();
                ResetAttributes();
            };
        }

        private void BindLabelToProperties(GameMonitorDisplayViewModel gameMonitorDisplayViewModel)
        {
            var gameNameBinding = new Binding("Text", gameMonitorDisplayViewModel, "GameName");
            labelGame.DataBindings.Add(gameNameBinding);
            
            var runningTimeBinding = new Binding("Text", gameMonitorDisplayViewModel, "ElapsedTime");
            labelTime.DataBindings.Add(runningTimeBinding);
        }

        private void HandleTimeExpired()
        {
            EnableCloseButton();
            ShowTimeInRed();
            PlayTimeExpiredWarningSound();
        }

        private void ResetAttributes()
        {
            soundPlayed = false;
            labelTime.ForeColor = DefaultForeColor;
            buttonClose.Enabled = false;
        }

        private void PlayTimeExpiredWarningSound()
        {
            if (soundPlayed) 
                return;
            
            SystemSounds.Exclamation.Play();
            soundPlayed = true;
        }

        private void ShowTimeInRed()
        {
            labelTime.ForeColor = Color.Red;
        }

        private void EnableCloseButton()
        {
            buttonClose.Enabled = true;
        }
    }
}
