using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tellurian.Trains.Clocks.Server.Tests")]

namespace Tellurian.Trains.Clocks.Server
{
    public enum PauseReason
    {
        None,
        Breakfast,
        Lunch,
        Dinner,
        Meeting,
        Closing,
        Other
    }
}
