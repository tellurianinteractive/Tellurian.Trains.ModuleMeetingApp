using System;
using System.Globalization;
using System.Reflection;
using Tellurian.Trains.MeetingApp.Shared;

namespace Tellurian.Trains.MeetingApp.Client
{
    public static class ClockStatusExtension
    {
        public static string StatusClass(this ClockStatus me)
        {
            if (me?.IsUnavailable != false) return "unavailable";
            if (me.IsRealtime) return "realtime";
            if (me.IsRunning)
            {
                return "fastclock";
            }
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
        public static string ClientVersionNumber => ClientVersion.ToString();
        public static bool IsClientVersionSameAsServer(this ClockStatus me) => me?.ServerVersionNumber.StartsWith(ClientVersion.ComparableVersionNumber(), StringComparison.Ordinal) == true;
        private static Version ClientVersion => Assembly.GetExecutingAssembly().GetName().Version;
        private static string ComparableVersionNumber(this Version me) => $"{me.Major}.{me.Minor}";
    }
}
