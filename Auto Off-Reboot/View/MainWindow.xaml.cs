using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Auto_Off_Reboot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan _subtractDate;
        private double _seconds;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded_CardsQuickTiming;
        }

        private void MainWindow_Loaded_CardsQuickTiming(object sender, RoutedEventArgs e)
        {
            ItemsControlCardsQuickTiming.ItemsSource = new QuickTiming[]
            {
                new QuickTiming(QuickTiming.OFF, 2, 0),
                new QuickTiming(QuickTiming.OFF, 2, 1),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 10),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
                new QuickTiming(QuickTiming.OFF, 0, 100),
            };
        }

        private void Click_Cancel_button(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void Click_Start_button(object sender, RoutedEventArgs e)
        {
            UpdateDate();

            ChoiceAction(Box.Text);
        }

        private void SnackbarTime()
        {
            SnackbarThree.MessageQueue.Enqueue(Box.Text + " через " + 
                   (_subtractDate.Hours == 0 ? "" : _subtractDate.Hours + " Чac. ") +
                   (_subtractDate.Minutes == 0 ? "" : _subtractDate.Minutes + " Мин. ") +
                   (_subtractDate.Seconds == 0 ? "" : _subtractDate.Seconds + " Сек."));
        }

        private void ChoiceAction(string choice)
        {
            Cancel();
            SnackbarTime();
            if (choice.Equals("Выключить")) Off();
            else Reboot();
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


        private void OpeningAccessStartButton()
        {
            if (!string.IsNullOrEmpty(Box.Text) && null != MyTimePicker.SelectedTime)
            {
                try
                {
                    UpdateDate();

                    StartButton.IsEnabled = true;

                    Timer myTimer = new Timer();
                    myTimer.Interval = _seconds * 1000;
                    myTimer.Elapsed += async (sender, e) => await HandleTimer();
                    myTimer.AutoReset = false;
                    myTimer.Enabled = true;
                    myTimer.Start();
                }
                catch (Exception e)
                {
                    MyTimePicker.Text = null;
                }


            }
            else StartButton.IsEnabled = false;

        }

        private void UpdateDate()
        {
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTimeGet = (DateTime)MyTimePicker.SelectedTime;
            _seconds = Math.Ceiling((dateTimeGet.Subtract(dateTimeNow)).TotalSeconds);
            if (_seconds < 0)
            {
                _seconds += 86_400;
                _subtractDate = dateTimeGet.AddSeconds(86_400).Subtract(dateTimeNow);
            }
            else _subtractDate = dateTimeGet.Subtract(dateTimeNow);


        }

        private async Task HandleTimer()
        {
            Dispatcher.Invoke(new Action(() => { StartButton.IsEnabled = false; }));
        }


        private void MyTimePicker_OnSelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            OpeningAccessStartButton();
        }

        private void ClickQuickTiming(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            QuickTiming data = b.DataContext as QuickTiming;
            SnackbarThree.MessageQueue.Enqueue(data.ActionType+" "+data.ActionTime+" "+data.Id);
        }
    }
}
