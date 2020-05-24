namespace Tellurian.Trains.MeetingApp.Client
{
    public class StoppingClockModel
    {
        public string Reason { get; set; } = "0";
        public string UserName { get; set; } = string.Empty;

        public bool HasReason => Reason != "0";
    }
}
