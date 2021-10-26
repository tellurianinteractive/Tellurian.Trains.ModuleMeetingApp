using System;
using System.Collections.Generic;
using System.Linq;
using Tellurian.Trains.Clocks.Contracts;

namespace Tellurian.Trains.MeetingApp.Contract.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<(int value, string display)> Weekdays => ((Weekday[])Enum.GetValues(typeof(Weekday))).Select(v => ((int)v, v.ToString()));
        public static IEnumerable<(int value, string display)> PauseReasons => ((PauseReason[])Enum.GetValues(typeof(PauseReason))).Select(v => ((int)v, v.ToString()));
        public static IEnumerable<(int value, string display)> StopReasons => ((StopReason[])Enum.GetValues(typeof(StopReason))).Select(v => ((int)v, v.ToString()));
        public static IEnumerable<(int value, string display)> Themes => ((Theme[])Enum.GetValues(typeof(Theme))).Select(v => ((int)v, v.ToString()));
        public static IEnumerable<(int value, string display)> Displays => ((Display[])Enum.GetValues(typeof(Display))).Select(v => ((int)v, v.ToString()));
    }
}
