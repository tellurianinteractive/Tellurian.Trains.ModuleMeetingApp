namespace Tellurian.Trains.MeetingApp.Clocks;
public interface ITimeProvider
{
    DateTimeOffset UtcNow { get; }
}

public class SystemTimeProvider : ITimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

