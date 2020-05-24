using System;
using System.Globalization;

namespace Tellurian.Trains.Clocks.Server
{
    public class ClockMessage
    {
        public string DefaultText { get; set; } = string.Empty;

#pragma warning disable IDE0060 // Remove unused parameter: Skall vara denna signatur för framtida bruk.
#pragma warning disable CA1801 // Review unused parameters
        public string ToString(CultureInfo culture) => DefaultText ?? string.Empty;
#pragma warning restore CA1801 // Review unused parameters
#pragma warning restore IDE0060 // Remove unused parameter
        public override string ToString() => DefaultText;
        

    }
}
