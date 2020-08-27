using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Auto_Off_Reboot.Annotations;
using Auto_Off_Reboot.View;
using MaterialDesignThemes.Wpf;

namespace Auto_Off_Reboot.ViewModel
{
    public class QuickTimingViewModel : QuickTiming, INotifyPropertyChanged
    {
        public bool IsButtonAdd { get; set; } = false;

        public event Action<int> EventClickItemQuickTiming;
        public event Action<int> EventDeleteItem;

        public delegate void DelAddItem();
        public event Action EventAddButtonAddItem;

        public ICommand ButtonClick { get; private set; }

        public ICommand DeleteMenuItem { get; set; }

        public ICommand RunDialogAddItem { get; set; }


            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }


            public DateTime ActionTime
            {
                get
                {
                    return DateTime.Now.Add(ActionSpanTime);
                }

            }

            public string StringTime
            {
                get
                {
                    return (ActionSpanTime.Hours == 0 ? "" : ActionSpanTime.Hours + " Чac. ") +
                           (ActionSpanTime.Minutes == 0 ? "" : ActionSpanTime.Minutes + " Мин. ");
                }


            }

             public QuickTimingViewModel(string actionType, int hours, int minutes) : base(actionType, hours, minutes)
             {
                 ButtonClick = new DelegateCommand(o => EventClickItemQuickTiming(Id)); 
                 DeleteMenuItem = new DelegateCommand(o => EventDeleteItem(Id));
             }

             public QuickTimingViewModel(QuickTiming quickTiming) : base(quickTiming)
             {
                 Id = quickTiming.Id;
                 ButtonClick = new DelegateCommand(o => EventClickItemQuickTiming(Id));
                 DeleteMenuItem = new DelegateCommand(o => EventDeleteItem(Id));
             }

            public QuickTimingViewModel()
            {
                IsButtonAdd = true;
                RunDialogAddItem = new DelegateCommand(o=> EventAddButtonAddItem());
            }

    }
}