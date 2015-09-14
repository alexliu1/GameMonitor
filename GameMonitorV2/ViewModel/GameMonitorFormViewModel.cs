﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using GameMonitorV2.Annotations;
using GameMonitorV2.Model;
using GameMonitorV2.View;

namespace GameMonitorV2.ViewModel
{
    public class GameMonitorFormViewModel : INotifyPropertyChanged
    {
        private ISynchronizeInvoke synchronizeInvoke;
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


        public GameMonitorFormViewModel(ISynchronizeInvoke synchronizeInvoke)
        {
            this.synchronizeInvoke = synchronizeInvoke;
            LoadGameToBeMonitored();
        }

        public void LoadGameToBeMonitored()
        {
            if (GameName == null) return;
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