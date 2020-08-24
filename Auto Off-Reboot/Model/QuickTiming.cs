using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Auto_Off_Reboot.Annotations;

namespace Auto_Off_Reboot
{
    public class QuickTiming
    {

        public const string OFF = "Выключить";
        public const string REBOOT = "Перезагрузить";

        public int Id { set; get; }
        public string ActionType
        { private set; get; }

        private TimeSpan ActionSpanTime;
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


        public QuickTiming(string actionType, int hours, int minutes)
        {
           
            ActionType = actionType;

            ActionSpanTime = new TimeSpan(hours, minutes, 0);
        }

        protected QuickTiming() {}
    }
}