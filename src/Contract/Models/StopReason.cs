namespace Tellurian.Trains.MeetingApp.Contracts.Models;

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
    VehicleBreakdown,
    Derailment,
    Other
}
