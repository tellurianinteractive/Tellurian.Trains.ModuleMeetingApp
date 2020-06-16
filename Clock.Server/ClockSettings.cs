using System;
using Tellurian.Trains.Clocks.Server;

namespace Tellurian.Trains.Clocks.Server
{
    public class ClockSettings
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public bool ShouldRestart { get; set; }
        public bool IsRunning { get; set; }
        public bool IsRealTime { get; set; }
        public Weekday? StartWeekday { get; set; }
        public TimeSpan? StartTime { get; set; }
        public double? Speed { get; set; }
        public double? DurationHours { get; set; }
        public TimeSpan? PauseTime { get; set; }
        public PauseReason PauseReason { get; set; }
        public TimeSpan? ExpectedResumeTime { get; set; }
        public bool ShowRealTimeWhenPaused { get; set; }
        public TimeSpan? OverriddenElapsedTime { get; set; }
        public ClockMessage? Message { get; set; }
        public TimeSpan TimeZoneOffset { get; set; } = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time").GetUtcOffset(DateTime.Now);
    }
}