using System.Windows;
using System.Windows.Controls;
using Auto_Off_Reboot.ViewModel;

namespace Auto_Off_Reboot.View
{
    public partial class DialogNewItemQuickTiming : UserControl
    {
        public DialogNewItemQuickTiming(DialogNewItemQuickTimingViewModel dialogNewItemQuickTimingViewModel)
        {
            InitializeComponent();
            DataContext = dialogNewItemQuickTimingViewModel;
        }
    }
}
