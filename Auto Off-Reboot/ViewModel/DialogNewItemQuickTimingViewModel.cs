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
                else _textBoxHours = null;
            }
        }

        private string _textBoxMinutes;
        public string TextBoxMinutes
        {
            get { return _textBoxMinutes; }
            set
            {
                int Num;
                if (int.TryParse(value, out Num) && (0 <= int.Parse(value) && int.Parse(value) < 60))
                    _textBoxMinutes = value;
                else _textBoxHours = null; 
                
            }
        }

        public ICommand ClickCancelButton { get; set; }

        public ICommand ClickCreateButton { get; set; }

        public DialogNewItemQuickTimingViewModel()
        {
            ClickCancelButton = new DelegateCommand(o => EventClickCancelButton());
            ClickCreateButton = new DelegateCommand(o =>
            {
                if (_textBoxHours == null) TextBoxHours = "0";
                if (_textBoxMinutes == null) TextBoxMinutes = "0";

                EventClickCreateButton(new QuickTimingViewModel(
                        TextComboBox,
                        int.Parse(TextBoxHours),
                        int.Parse(TextBoxMinutes)));


                });
        }


    }
}