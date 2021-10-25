using System;
using System.Globalization;

namespace Tellurian.Trains.MeetingApp.Contract
{
    public static class ClockStatusExtensions
    {
        public static string StatusClass(this ClockStatus me)
        {
            if (me?.IsUnavailable != false) return "unavailable";
            if (me.IsRealtime) return "realtime";
            if (me.IsRunning) return "fastclock";
            return "stopped";
        }

        public static string IsStopped(this ClockStatus me)
        {
            if (me?.IsRunning == true) return "disabled";
            return "";
        }
        public static string IsStarted(this ClockStatus me)
        {
            if (me?.IsRunning == true) return "";
            return "disabled";
        }
        private static double MinutesPerGameHour(this ClockStatus me) => 60 / (me.Speed < 0 ? 1 : me.Speed);
        public static double MinutesPerHour(this ClockStatus me) => Math.Floor( me.MinutesPerGameHour());
        public static double SecondsReminderPerHour(this ClockStatus me)
        {
            var gameHour = me.MinutesPerGameHour();
            return (gameHour - Math.Floor(gameHour)) * 60;
        }
    }
}
