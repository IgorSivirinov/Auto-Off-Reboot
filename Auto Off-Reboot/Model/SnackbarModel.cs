using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Auto_Off_Reboot.Annotations;

namespace Auto_Off_Reboot
{
    public class SnackbarModel : INotifyPropertyChanged
    {
        public bool IsActive { get; private set; } = false;
        public string Content { get; private set; } = "";

        public async void MessageShow(string message)
        {
            Content = message;
            OnPropertyChanged("Content");

            IsActive = true;
            OnPropertyChanged("IsActive");

            await Task.Run(() => Thread.Sleep(5000));

           IsActive = false;
           OnPropertyChanged("IsActive");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}