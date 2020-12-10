using System;
using Tellurian.Trains.Clocks.Contracts;

namespace Tellurian.Trains.MeetingApp.Contract.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string me) => 
            !string.IsNullOrWhiteSpace(me);

        public static TimeSpan? AsTimeSpanOrNull(this string time) =>
            TimeSpan.TryParse(time, out var value) ? value : (TimeSpan?)null;

        public static PauseReason AsPauseReason(this string me) =>
            Enum.TryParse<PauseReason>(me, ignoreCase: true, out var value) ? value : PauseReason.NoReason;

        public static StopReason AsStopReason(this string me) =>
            Enum.TryParse<StopReason>(me, ignoreCase: true, out var value) ? value : StopReason.SelectStopReason;

        public static bool IsInvalid(this StopReason me) =>
            me == StopReason.SelectStopReason;

        public static Theme AsTheme(this string me) =>
            Enum.TryParse<Theme>(me, ignoreCase: true, out var value) ? value : Theme.Light;
    }
}
