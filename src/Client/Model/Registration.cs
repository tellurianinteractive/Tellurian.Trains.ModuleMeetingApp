namespace Tellurian.Trains.MeetingApp.Client.Model;

public class Registration
{
    public string? UserName { get; set; }
    public required string ClockName { get; set; }
    public string? ClockPassword { get; set; }
    public required string Theme { get; set; } 
    public required string Display { get; set; } 
    public bool IsInstructionVisible { get; set; }
    public bool DisplayTimeMaximized { get; set; }
    public bool ShowSecondHand { get; set; }
    public string? PreferredLanguage { get; set; }

    public const string Key = "registration";

    public static Registration Default => new()
    {
        ClockName = ClockSettings.DemoClockName,
        ClockPassword = ClockSettings.DemoClockPassword,
        Theme = ClockSettings.DefaultTheme,
        Display = ClockSettings.DefaultDisplay,
        IsInstructionVisible = true
    };
}
