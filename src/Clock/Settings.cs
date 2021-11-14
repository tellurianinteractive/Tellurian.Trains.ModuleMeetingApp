using Tellurian.Trains.MeetingApp.Contract.Model;

namespace Tellurian.Trains.MeetingApp.Clocks;

public class Settings
{
    public static string DefaultName => "Demo";
    public static string DefaultPassword => "password";
    private static TimeSpan DefaultStartTime => TimeSpan.FromHours(6);

    public string? Name { get; set; } = DefaultName;
    public string? AdministratorPassword { get; set; } = DefaultPassword;
    public string? UserPassword { get; set; } = string.Empty;
    public bool ShouldRestart { get; set; }
    public bool IsRunning { get; set; }
    public bool IsRealtime { get; set; }
    public Weekday? StartWeekday { get; set; }
    public TimeSpan? StartTime { get; set; } = DefaultStartTime;
    public TimeSpan FastTime { get; set; } = DefaultStartTime;
    public TimeSpan RealTime { get; set; }
    public TimeSpan CurrentTime { get; set; }
    public double? Speed { get; set; }
    public TimeSpan? Duration { get; set; }
    public TimeSpan? PauseTime { get; set; }
    public PauseReason PauseReason { get; set; }
    public TimeSpan? ExpectedResumeTime { get; set; }
    public bool ShowRealTimeWhenPaused { get; set; }
    public TimeSpan? OverriddenElapsedTime { get; set; }
    public Message? Message { get; set; }
    public TimeSpan TimeZoneOffset { get; set; } = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time").GetUtcOffset(DateTime.Now.Date);
    
}
