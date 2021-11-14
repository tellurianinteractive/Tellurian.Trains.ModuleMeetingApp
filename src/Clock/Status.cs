using Tellurian.Trains.MeetingApp.Contract.Model;

namespace Tellurian.Trains.MeetingApp.Clocks;

/// <summary>
/// Data for presentation of clock time and status.
/// All data is in English-only. It is the clients responibility to make translations.
/// </summary>
public record Status
{
    /// <summary>
    /// Name of selected clock.
    /// </summary>
    public string Name { get; init; } = string.Empty;
    /// <summary>
    /// Name of game weekday or empty.
    /// </summary>
    public Weekday Weekday { get; init; } = Weekday.NoDay;
    /// <summary>
    /// Current time to display. The type of time is dependent of <see cref="IsRealtime"/>.
    /// </summary>
    public TimeSpan Time { get; init; }
    /// <summary>
    /// Game duration in hours (with decimals).
    /// </summary>
    public TimeSpan Duration { get; init; }
    /// <summary>
    /// True if clock is running. This is always true if clock is running in realtime.
    /// </summary>
    public bool IsRunning { get; init; }
    /// <summary>
    /// Indicates that the clock <see cref="Time"/> is in realtime; otherwise it is running in game time.
    /// </summary>
    public bool IsRealtime { get; init; }
    /// <summary>
    /// True if game time has reached the end time. Always false when running in realtime.
    /// </summary>
    public bool IsCompleted { get; init; }
    /// <summary>
    /// Eventually manual entered message by the administrator to display.
    /// </summary>
    public string Message { get; init; } = string.Empty;
    /// <summary>
    /// Clock speed where values over 1 is faster than real time.
    /// </summary>
    public double Speed { get; init; }
    /// <summary>
    /// Shoud be set to true when API-data not is available.
    /// </summary>
    public bool IsUnavailable { get; init; }
    /// <summary>
    /// Forecasted real end time of game- Includes time of pause if pause times have been set.
    /// </summary>
    public TimeSpan RealEndTime { get; init; }
    /// <summary>
    /// Game end time.
    /// </summary>
    public TimeSpan FastEndTime { get; init; }
    /// <summary>
    /// True if game time is paused. It is recommended to present when and why the game is paused and will resume.
    /// </summary>
    public bool IsPaused { get; init; }
    /// <summary>
    /// Reason for pause. These are from the set of values in <see cref="PauseReason"/>.
    /// </summary>
    public PauseReason PauseReason { get; init; }
    /// <summary>
    /// Real time when pause starts.
    /// </summary>
    public TimeSpan? PauseTime { get; init; }
    /// Expected real time when game will resume or empty.
    /// </summary>
    public TimeSpan? ExpectedResumeTimeAfterPause { get; init; }
    /// <summary>
    /// Name of user or station that have stopped the game time.
    /// </summary>
    public string StoppedByUser { get; init; } = string.Empty;
    /// <summary>
    /// Reason for stopping game time. These are from the set of values in <see cref="StoppingReason"/>.
    /// </summary>
    public StopReason StoppingReason { get; init; }
    /// <summary>
    /// Current server application version. This can be used to verify client application compatibility.
    /// </summary>
    public string ServerVersionNumber { get; init; } = string.Empty;
}
