using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Tellurian.Trains.Clocks.Server;

namespace Tellurian.Trains
{
    public static class StringExtensions
    {
        public static bool HasValue(this string me)
        {
            return !string.IsNullOrWhiteSpace(me);
        }

        public static string AsTime(this TimeSpan me)
        {
            return me.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
        }

        public static string AsTimeOrEmpty(this TimeSpan? me)
        {
            if (me.HasValue && me.Value > TimeSpan.Zero) return me.Value.AsTime();
            return string.Empty;
        }

        public static TimeSpan? AsTimeSpanOrNull(this string time) =>
            TimeSpan.TryParse(time, out var value) ? value : (TimeSpan?)null;

        public static PauseReason AsPauseReason(this string me) =>
            Enum.TryParse<PauseReason>(me, ignoreCase: true, out var value) ? value : PauseReason.None;

        public static StopReason AsStopReason(this string me) =>
                        Enum.TryParse<StopReason>(me, ignoreCase: true, out var value) ? value : StopReason.Unknown;
    }
}
