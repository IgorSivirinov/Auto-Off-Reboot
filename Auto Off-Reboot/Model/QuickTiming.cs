using System;

namespace Auto_Off_Reboot
{
    public class QuickTiming
    {
        public int Id { get;  set; }

        public string ActionType { set; get; }

        public TimeSpan ActionSpanTime { get; set; }

        public QuickTiming( string actionType, int hours, int minutes)
        {
            ActionType = actionType;
            ActionSpanTime = new TimeSpan(hours, minutes, 0);
        }

        protected QuickTiming(QuickTiming quickTiming)
        {
            ActionType = quickTiming.ActionType;
            ActionSpanTime = quickTiming.ActionSpanTime;
        }

        
        protected QuickTiming() {}
    }
}