namespace Tellurian.Trains.MeetingApp.Client.Model;

public class StoppingClockModel
{
    public string Reason { get; set; } = "0";
    public string UserName { get; set; } = string.Empty;

    public bool HasReason => Reason != "0";
}
