using Auto_Off_Reboot.Annotations;
using Auto_Off_Reboot.Data;
using Auto_Off_Reboot.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Auto_Off_Reboot.ViewModel
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private TimeSpan _subtractDate;
        private double _seconds;

        public event PropertyChangedEventHandler PropertyChanged;

        public SnackbarModel Snackbar { get; set; } = new SnackbarModel();

        public object SelectedTimeTimePicker { get; set; }

        public string TextComboBoxChoiceAction { set; get; }

        private string _textTimePicker;
        public string TextTimePicker
        {
            get { return _textTimePicker; }

            set{
                _textTimePicker = value;
                OpeningAccessStartButton();
            }
        }


        public bool IsEnabledButtonStart { get; set; } = false;

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get { return _isDialogOpen; }
            set
            {
                if (_isDialogOpen == value) return;
                _isDialogOpen = value;
                OnPropertyChanged();
            }
        }

        private object _dialogContent;
        public object DialogContent
        {
            get { return _dialogContent; }
            set
            {
                if (_dialogContent == value) return;
                _dialogContent = value;
                OnPropertyChanged();
            }
        }



        public ObservableCollection<QuickTimingViewModel> QuickTimings { get; set; }
            = new ObservableCollection<QuickTimingViewModel>();

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        public MainWindowViewModel()
        {
            LoadingItemsQuickTiming();
        }




        private void LoadingItemsQuickTiming()
        {
            QuickTimings.Clear();
            using (var context = new DatabaseContext())
            { 
                foreach (var itemQuickTiming in context.QuickTimings)
                {
                    QuickTimingViewModel itemQuickTimingViewModel = new QuickTimingViewModel(itemQuickTiming);
                    itemQuickTimingViewModel.EventClickItemQuickTiming += Item_EventClickItemQuickTiming;
                    itemQuickTimingViewModel.EventDeleteItem += Item_EventDeleteItem;

                    QuickTimings.Add(itemQuickTimingViewModel);
                }

                if (QuickTimings.Count == 0)
                {
                    QuickTimingViewModel itemAdd = new QuickTimingViewModel();
                    itemAdd.EventAddButtonAddItem += ItemAdd_EventAddItem;
                    QuickTimings.Add(itemAdd);
                }
                else
                {
                    if (QuickTimings.Count <= 5 && !QuickTimings[QuickTimings.Count - 1].IsButtonAdd)
                    {
                        QuickTimingViewModel itemAdd = new QuickTimingViewModel();
                        itemAdd.EventAddButtonAddItem += ItemAdd_EventAddItem;
                        QuickTimings.Add(itemAdd);
                    }
                }

                
            }
        }

        private void AddItemQuickTiming(QuickTiming itemQuickTiming)
        {
            using (var context = new DatabaseContext())
            {
                context.QuickTimings.Add(itemQuickTiming);
                context.SaveChanges();
            }
            LoadingItemsQuickTiming();
        }

        private void Item_EventDeleteItem(int id)
        {
            using (var context = new DatabaseContext())
            {
                var deleteQuickTiming = context.QuickTimings.Find(id);
                context.QuickTimings.Remove(deleteQuickTiming);

                context.SaveChanges();
            }
            LoadingItemsQuickTiming();
        }

        private void ItemAdd_EventAddItem()
        {
            DialogNewItemQuickTimingViewModel dialog = new DialogNewItemQuickTimingViewModel();
            dialog.EventClickCancelButton += Dialog_EventClickCancelButton;
            dialog.EventClickCreateButton += Dialog_EventClickCreateButton;
            DialogContent = new DialogNewItemQuickTiming(dialog);
            IsDialogOpen = true;
        }


        private void Dialog_EventClickCreateButton(QuickTimingViewModel obj)
        {
            AddItemQuickTiming(obj);
            IsDialogOpen = false;
        }

        private void Dialog_EventClickCancelButton()
        {
            IsDialogOpen = false;
        }


        private void Item_EventClickItemQuickTiming(int id)
        {
            using (var context = new DatabaseContext())
            {
                QuickTimingViewModel quickTimingViewModel 
                    = new QuickTimingViewModel(context.QuickTimings.Find(id));

                UpdateDate(quickTimingViewModel.ActionTime);
                ChoiceAction(quickTimingViewModel.ActionType);
            }
        }



        public ICommand ButtonClickStart
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    UpdateDate((DateTime)SelectedTimeTimePicker);
                    ChoiceAction(TextComboBoxChoiceAction);
                });
            }
        }

        public ICommand ButtonClickCancel
        {
            get
            {
                return new DelegateCommand((obj) => Cancel());
            }
        }



        private void SnackbarTime(string choice)
        {
            Snackbar.MessageShow(choice + " через " +
                                 (_subtractDate.Hours == 0 ? "" : _subtractDate.Hours + " Чac. ") +
                                 (_subtractDate.Minutes == 0 ? "" : _subtractDate.Minutes + " Мин. ") +
                                 (_subtractDate.Seconds == 0 ? "" : _subtractDate.Seconds + " Сек."));
        }

        private void ChoiceAction(string choice)
        {
            Cancel();
            SnackbarTime(choice);
            if (choice.Equals("Выключить")) Off();
            else Reboot();
        }



        private void OpeningAccessStartButton()
        {
            if (!string.IsNullOrEmpty(TextComboBoxChoiceAction) && null != SelectedTimeTimePicker)
            {
                try
                {
                    UpdateDate((DateTime)SelectedTimeTimePicker);

                    IsEnabledButtonStart = true;
                    OnPropertyChanged("IsEnabledButtonStart");

                    Timer myTimer = new Timer();
                    myTimer.Interval = _seconds * 1000;
                    myTimer.Elapsed += async (sender, e) =>
                        await Task.Run(() => IsEnabledButtonStart = false);
                    myTimer.AutoReset = false;
                    myTimer.Enabled = true;
                    myTimer.Start();
                }
                catch (Exception e)
                {
                    TextTimePicker = null;
                }


            }
            else
            {
                IsEnabledButtonStart = false;
                OnPropertyChanged("IsEnabledButtonStart");
            }

        }

        private void UpdateDate(DateTime dateTimeGet)
        {
            DateTime dateTimeNow = DateTime.Now;
            _seconds = Math.Ceiling((dateTimeGet.Subtract(dateTimeNow)).TotalSeconds);
            if (_seconds < 0)
            {
                _seconds += 86_400;
                _subtractDate = dateTimeGet.AddSeconds(86_400).Subtract(dateTimeNow);
            }
            else _subtractDate = dateTimeGet.Subtract(dateTimeNow);


        }


        private void Off()
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = @"/k shutdown -s -t " + _seconds,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
        }

        private void Reboot()
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = @"/k shutdown -r -t " + _seconds,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
        }

        private void Cancel()
        {
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = @"/k shutdown -a",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
        }
    }
}