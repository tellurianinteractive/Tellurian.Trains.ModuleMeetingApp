using Tellurian.Trains.MeetingApp.Contracts;

namespace Tellurian.Trains.MeetingApp.Client.Model;

public class Registration
{
    public string? UserName { get; set; }
    public string ClockName { get; set; } = ClockSettings.DemoClockName;
    public string? ClockPassword { get; set; } = ClockSettings.DemoClockPassword;
    public string Theme { get; set; } = ClockSettings.DefaultTheme;
    public string Display { get; set; } = ClockSettings.DefaultDisplay;
    public bool IsInstructionVisible { get; set; } = true;
    public bool DisplayTimeMaximized { get; set; }

    public const string Key = "registration";
}
