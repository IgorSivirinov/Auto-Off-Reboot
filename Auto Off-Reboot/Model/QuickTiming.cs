using System;

namespace Auto_Off_Reboot
{
    public class QuickTiming
    {
        

        public const string OFF = "Выключить";
        public const string REBOOT = "Перезагрузить";

        private static int _maxId = 0;
        public int Id { set; get; }
        public string ActionType
        { private set; get; }

        private TimeSpan _actionTime;
        public string ActionTime
        {
            get
            {
                return (_actionTime.Hours == 0 ? "" : _actionTime.Hours + " Чac. ") +
                       (_actionTime.Minutes == 0 ? "" : _actionTime.Minutes + " Мин. ");
            }

        }

        public QuickTiming(string actionType, int hours, int minutes)
        {
            ActionType = actionType;
            _actionTime = new TimeSpan(hours, minutes, 0);
            Id = _maxId++;
        }
    }
}