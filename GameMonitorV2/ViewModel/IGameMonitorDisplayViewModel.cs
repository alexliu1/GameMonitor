using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GameMonitorV2.Model;
using log4net;

namespace GameMonitorV2.ViewModel
{
    public interface IGameMonitorDisplayViewModel: ISynchronizeInvoke
    {
        //private static TimeSpan TimeLimit = TimeSpan.FromHours(3);
        event Action TimeExpired; 

        void LoadGameToBeMonitored(string fileNameAndPath);

        void RaiseTimeExpiredIfNecessary(TimeSpan value);

        event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        void OnPropertyChanged([CallerMemberName] string propertyName = null);

        void RaiseOnUiThread(MethodInvoker methodToRaise);

        //public bool TryCloseProgram(out string errorMessage)
        void CloseProgram();
    }
}