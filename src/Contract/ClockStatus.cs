namespace Tellurian.Trains.MeetingApp.Contracts;

/// <summary>
/// Data for presentation of clock time and status.
/// All data is in English-only. It is the clients responibility to make translations.
/// </summary>
public class ClockStatus
{
    /// <summary>
    /// Name of selected clock.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Name of game weekday or empty.
    /// </summary>
    public string Weekday { get; set; } = string.Empty;
    /// <summary>
    /// Current time to display. The type of time is dependent of <see cref="IsRealtime"/>.
    /// </summary>
    public string Time { get; set; } = string.Empty;
    /// <summary>
    /// Game duration in hours (with decimals).
    /// </summary>
    public double Duration { get; set; }
    /// <summary>
    /// True if clock is running. This is always true if clock is running in realtime.
    /// </summary>
    public bool IsRunning { get; set; }
    /// <summary>
    /// Indicates that the clock <see cref="Time"/> is in realtime; otherwise it is running in game time.
    /// </summary>
    public bool IsRealtime { get; set; }
    /// <summary>
    /// True if game time has reached the end time. Always false when running in realtime.
    /// </summary>
    public bool IsCompleted { get; set; }
    /// <summary>
    /// Eventually manual entered message by the administrator to display.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Clock speed where values over 1 is faster than real time.
    /// </summary>
    public double Speed { get; set; }
    /// <summary>
    /// Shoud be set to true when API-data not is available.
    /// </summary>
    public bool IsUnavailable { get; set; }
    /// <summary>
    /// Forecasted real end time of game- Includes time of pause if pause times have been set.
    /// </summary>
    public string RealEndTime { get; set; } = string.Empty;
    /// <summary>
    /// Game end time.
    /// </summary>
    public string FastEndTime { get; set; } = string.Empty;
    /// <summary>
    /// True if game time is paused. It is recommended to present when and why the game is paused and will resume.
    /// </summary>
    public bool IsPaused { get; set; }
    /// <summary>
    /// Reason for pause. These are from the set of values in <see cref="PauseReason"/>.
    /// </summary>
    public string PauseReason { get; set; } = string.Empty;
    /// <summary>
    /// Real time when pause starts.
    /// </summary>
    public string PauseTime { get; set; } = string.Empty;
    /// <summary>
    /// Expected real time when game will resume or empty.
    /// </summary>
    public string ExpectedResumeTimeAfterPause { get; set; } = string.Empty;
    /// <summary>
    /// Name of user or station that have stopped the game time.
    /// </summary>
    public string StoppedByUser { get; set; } = string.Empty;
    /// <summary>
    /// Reason for stopping game time. These are from the set of values in <see cref="StoppingReason"/>.
    /// </summary>
    public string StoppingReason { get; set; } = string.Empty;
    /// <summary>
    /// Current server application version. This can be used to verify client application compatibility.
    /// </summary>
    public string ServerVersionNumber { get; set; } = string.Empty;
}
