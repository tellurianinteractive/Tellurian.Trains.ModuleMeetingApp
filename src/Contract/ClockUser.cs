namespace Tellurian.Trains.MeetingApp.Contracts;

public class ClockUser
{
    public string IPAddress { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string? ClientVersion { get; set; } = string.Empty;
    public string LastUsedTime { get; set; } = string.Empty;
}
