using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Auto_Off_Reboot.Annotations;

namespace Auto_Off_Reboot.ViewModel
{
    public class DialogNewItemQuickTimingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event Action<QuickTimingViewModel> EventClickCreateButton;

        public event Action EventClickCancelButton;


        public string TextComboBox { get; set; } = "Выключить";

        private string _textBoxHours;
        public string TextBoxHours
        {
            get { return _textBoxHours; }
            set
            {
                int Num;
                if (int.TryParse(value, out Num) && (0 <= int.Parse(value) && int.Parse(value) < 24))
                    _textBoxHours = value;
                else _textBoxHours = "0";
                OnPropertyChanged("TextBoxHours");
            }
        }

        private string _textBoxMinutes;
        public string TextBoxMinutes
        {
            get { return _textBoxMinutes; }
            set
            {
                int Num;
                if (int.TryParse(value, out Num) && ( 0 <= int.Parse(value) && int.Parse(value) < 60))
                    _textBoxMinutes = value;
                else _textBoxMinutes = "0";
                OnPropertyChanged("TextBoxMinutes");
            }
        }

        public ICommand ClickCancelButton { get; set; }

        public ICommand ClickCreateButton { get; set; }

        public DialogNewItemQuickTimingViewModel()
        {
            TextBoxMinutes = "0";
            TextBoxHours = "0";
            ClickCancelButton = new DelegateCommand(o => EventClickCancelButton());
            ClickCreateButton = new DelegateCommand(o =>
                EventClickCreateButton(new QuickTimingViewModel(
                        TextComboBox,
                        int.Parse(TextBoxHours), 
                        int.Parse(TextBoxMinutes))));
        }


    }
}