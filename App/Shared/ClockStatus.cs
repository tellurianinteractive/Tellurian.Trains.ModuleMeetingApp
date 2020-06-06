namespace Tellurian.Trains.MeetingApp.Shared
{
    public class ClockStatus
    {
        public string Name { get; set; } = string.Empty;
        public string Weekday { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public double Duration { get; set; }
        public bool IsRunning { get; set; }
        public bool IsRealtime { get; set; }
        public bool IsCompleted { get; set; }
        public string Message { get; set; } = string.Empty;
        public double Speed { get; set; }
        public bool IsUnavailable { get; set; }
        public string RealEndTime { get; set; } = string.Empty;
        public string FastEndTime { get; set; } = string.Empty;
        public bool IsPaused { get; set; }
        public string PauseReason { get; set; } = string.Empty;
        public string PauseTime { get; set; } = string.Empty;
        public string ExpectedResumeTimeAfterPause { get; set; } = string.Empty;
        public string StoppedByUser { get; set; } = string.Empty;
        public string StoppingReason { get; set; } = string.Empty;
    }
}
