namespace Tellurian.Trains.MeetingApp.Contract.Model;

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
