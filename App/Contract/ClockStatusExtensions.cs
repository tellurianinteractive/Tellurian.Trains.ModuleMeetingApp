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

        public static string SecondsPerHour(this ClockStatus me)
        {
            var speed = me == null ? 0 : me.Speed;
            return (60 / speed).ToString("F0", CultureInfo.CurrentCulture);
        }
        //public static bool IsClientVersionSameAsServer(this ClockStatus me) => me?.ServerVersionNumber.StartsWith(ClientVersion.ComparableVersionNumber(), StringComparison.Ordinal) == true;
        private static string ComparableVersionNumber(this Version me) => $"{me.Major}.{me.Minor}";
    }
}
