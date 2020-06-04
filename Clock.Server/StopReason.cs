using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tellurian.Trains.Clocks.Server.Tests")]

namespace Tellurian.Trains.Clocks.Server
{
    public enum StopReason
    {
        SelectStopReason,
        StationControl,
        PointProblem,
        TrackProblem,
        CablingError,
        BoosterError,
        LocoNetError,
        CentralError,
        Delays,
        DriverShortage,
        Derailment,
        Other
    }
}
