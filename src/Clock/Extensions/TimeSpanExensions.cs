namespace Tellurian.Trains.MeetingApp.Clocks;

public static class TimeSpanExensions
{
    public static TimeSpan AsTimeOnly(this TimeSpan time) => new (time.Hours, time.Minutes, time.Seconds);
}
