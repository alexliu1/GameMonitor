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
        private ISynchronizeInvoke synchronizeInvoke;
        private string gameName;
        private TimeSpan elapsedTime;
        private ILog logger;

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
                logger.Debug(string.Format("Process: [{0}] Running time: [{1}]", GameName, value));
            }
        }

        public GameMonitorDisplayViewModel(ISynchronizeInvoke synchronizeInvoke, string fileNameAndPath, Func<Type, ILog> loggerFactory)
        {
            this.synchronizeInvoke = synchronizeInvoke;
            logger = loggerFactory(typeof(GameMonitorDisplayViewModel));

            LoadGameToBeMonitored(fileNameAndPath, loggerFactory);
        }

        private void LoadGameToBeMonitored(string fileNameAndPath, Func<Type, ILog> loggerFactory)
        {
            GameName = Path.GetFileNameWithoutExtension(fileNameAndPath);
            var watchingGame = ModelFactory.CreateNewPollWatcher(GameName, loggerFactory);
            watchingGame.ElapsedTimeTick += () => { ElapsedTime = watchingGame.ElapsedTime; };
            logger.Info(string.Format("Monitoring process [{0}]", GameName));
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

        //public bool TryCloseProgram(out string errorMessage)
        public void CloseProgram()
        {
            if (GameName == null) return;
            logger.InfoFormat("Closing process [{0}]", GameName);

            try
            {
                var program = Process.GetProcessesByName(GameName).First();
                program.Kill();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error when closing [{0}]", GameName), ex);
            } 
        }
    }
}