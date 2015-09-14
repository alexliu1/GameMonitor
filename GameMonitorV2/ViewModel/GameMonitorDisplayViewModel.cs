using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GameMonitorV2.Annotations;
using GameMonitorV2.Model;

namespace GameMonitorV2.View
{
    public class GameMonitorDisplayViewModel : INotifyPropertyChanged
    {
        private readonly ISynchronizeInvoke synchronizeInvoke;
        private string gameName;
        private TimeSpan elapsedTime;

        public string GameName
        {
            get { return gameName; }
            set { gameName = value; OnPropertyChanged(); }
        }

        public TimeSpan ElapsedTime
        {
            get { return elapsedTime; }
            set
            {
                if (value == elapsedTime)
                    return;
                elapsedTime = value;
                OnPropertyChanged();
            }
        }
        public GameMonitorDisplayViewModel(ISynchronizeInvoke synchronizeInvoke, string fileNameAndPath)
        {
            this.synchronizeInvoke = synchronizeInvoke;

            LoadGameToBeMonitored(fileNameAndPath);
        }

        private void LoadGameToBeMonitored(string fileNameAndPath)
        {
            GameName = Path.GetFileNameWithoutExtension(fileNameAndPath);
            var watchingGame = new PollWatcher(GameName);
            watchingGame.ElapsedTimeTick += () => { ElapsedTime = watchingGame.ElapsedTime; };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (synchronizeInvoke.InvokeRequired)
                synchronizeInvoke.BeginInvoke(new MethodInvoker(() => OnPropertyChanged(propertyName)), null);
            else
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}