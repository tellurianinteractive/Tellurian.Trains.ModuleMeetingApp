namespace Tellurian.Trains.Clocks.Contracts
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
        VehicleBreakdown,
        Derailment,
        Other
    }
}
