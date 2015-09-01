using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using GameMonitorV2.View;

namespace GameMonitorV2.ViewModel
{
    public class GameMonitorFormViewModel : INotifyPropertyChanged
    {
        private readonly ISynchronizeInvoke synchronizeInvoke;

        public GameMonitorFormViewModel(ISynchronizeInvoke synchronizeInvoke)
        {
            this.synchronizeInvoke = synchronizeInvoke;


        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}