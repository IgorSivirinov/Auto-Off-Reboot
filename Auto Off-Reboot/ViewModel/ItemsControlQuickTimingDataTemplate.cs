using System.Windows;
using System.Windows.Controls;

namespace Auto_Off_Reboot.ViewModel
{
    public class ItemsControlQuickTimingDataTemplate : DataTemplateSelector
    {
        public DataTemplate ItemQuickTiming { get; set; }
        public DataTemplate ButtonAddItemQuickTiming { get; set; }

        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            if (((QuickTimingViewModel)item).IsButtonAdd) return ButtonAddItemQuickTiming;
            return ItemQuickTiming;
        }
    }
}