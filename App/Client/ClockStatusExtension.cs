using System.Globalization;
using Tellurian.Trains.MeetingApp.Shared;

namespace Tellurian.Trains.MeetingApp.Client
{
    public static class ClockStatusExtension
    {
        public static string StatusClass(this ClockStatus me)
        {
            if (me == null || me.IsUnavailable) return "unavailable";
            if (me.IsRealtime) return "realtime";
            if (me.IsRunning)
            {
                return "fastclock";
            }
            return "stopped";
        }

        public static string IsStopped(this ClockStatus me)
        {
            if (me != null && me.IsRunning) return "disabled";
            return "";
        }
        public static string IsStarted(this ClockStatus me)
        {
            if (me != null && me.IsRunning) return "";
            return "disabled";
        }

        public static string SecondsPerHour(this ClockStatus me)
        {
            var speed = me == null ? 0 : me.Speed;
            return (60 / speed).ToString("F0", CultureInfo.CurrentCulture);
        }
    }
}
