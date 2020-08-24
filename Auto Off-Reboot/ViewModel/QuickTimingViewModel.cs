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

        public delegate void ClickItemQuickTiming(int id);
        public event ClickItemQuickTiming EventClickItemQuickTiming;
        public event ClickItemQuickTiming EventDeleteItem;

        public delegate void DelAddItem();
        public event DelAddItem EventAddItem;

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public ICommand ButtonClick { get; private set; }

            public ICommand AddItem { get; set; }

            public ICommand ButtonRightClick { get; set; }

            public ICommand DeleteMenuItem { get; set; }

            public ICommand RunDialogAddItem { get; set; }



            public QuickTimingViewModel(string actionType, int hours, int minutes) : base(actionType, hours, minutes)
            {
                ButtonClick = new DelegateCommand(o => EventClickItemQuickTiming(Id));
                DeleteMenuItem = new DelegateCommand(o => EventDeleteItem(Id));
            }

            public QuickTimingViewModel()
            {
                IsButtonAdd = true;

                RunDialogAddItem = new DelegateCommand(o=> EventAddItem());
            }

    }
}