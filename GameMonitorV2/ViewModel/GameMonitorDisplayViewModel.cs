using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GameMonitorV2.Model;
using log4net;

namespace GameMonitorV2.ViewModel
{
    public class GameMonitorDisplayViewModel : INotifyPropertyChanged
    {
        //private static TimeSpan TimeLimit = TimeSpan.FromHours(3);
        private static TimeSpan TimeLimit = TimeSpan.FromSeconds(10);
        public event Action TimeExpired; 
        private readonly ISynchronizeInvoke synchronizeInvoke;
        private string gameName;
        private TimeSpan elapsedTime;
        private ILog log;

        public string GameName
        {
            get { return gameName; }
            set
            {
                gameName = value;
                OnPropertyChanged();
            }
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
                RaiseTimeExpiredIfNecessary(value);    
            }
        }

        public GameMonitorDisplayViewModel(ISynchronizeInvoke synchronizeInvoke, string fileNameAndPath, ILog log)
        {
            this.synchronizeInvoke = synchronizeInvoke;
            this.log = log;

            LoadGameToBeMonitored(fileNameAndPath);
        }

        private void LoadGameToBeMonitored(string fileNameAndPath)
        {
            GameName = Path.GetFileNameWithoutExtension(fileNameAndPath);
            var watchingGame = new PollWatcher(GameName);
            watchingGame.ElapsedTimeTick += () => { ElapsedTime = watchingGame.ElapsedTime; };
            log.Info(string.Format("Monitoring started on: {0}", GameName));
        }

        private void RaiseTimeExpiredIfNecessary(TimeSpan value)
        {
            if (value >= TimeLimit && TimeExpired != null)
                RaiseOnUiThread(() => TimeExpired());
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            MethodInvoker methodToRaise = () => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            RaiseOnUiThread(methodToRaise);
        }

        private void RaiseOnUiThread(MethodInvoker methodToRaise)
        {
            if (synchronizeInvoke.InvokeRequired)
                synchronizeInvoke.BeginInvoke(methodToRaise, null);
            else
                methodToRaise();
        }

        public void CloseProgram()
        {
            if (GameName == null) return;
            var program = Process.GetProcessesByName(GameName).First();
            log.Info(string.Format("Close Command sent to {0}", GameName));
            try
            {
                program.Kill();
            }
            catch (Exception ex)
            {
                log.Info(string.Format("Error during {0} closing", GameName), ex);
            } 
        }
    }
}