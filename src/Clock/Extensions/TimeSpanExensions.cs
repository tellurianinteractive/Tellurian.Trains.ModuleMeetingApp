namespace Tellurian.Trains.MeetingApp.Clocks;

public static class TimeSpanExensions
{
    public static TimeSpan AsTimeOnly(this TimeSpan time) => new (time.Hours, time.Minutes, time.Seconds);

    /// <summary>
    /// The time of day part of a day-and-time value, including fractions of seconds.
    /// </summary>
    public static TimeSpan TimeOfDay(this TimeSpan time) => TimeSpan.FromTicks(((time.Ticks % TimeSpan.TicksPerDay) + TimeSpan.TicksPerDay) % TimeSpan.TicksPerDay);

    /// <summary>
    /// True if a time of day occurs in the interval after <paramref name="from"/> up to and including <paramref name="to"/>,
    /// also when the interval passes midnight.
    /// </summary>
    public static bool IsPassedBetween(this TimeSpan timeOfDay, TimeSpan from, TimeSpan to)
    {
        var interval = to - from;
        if (interval <= TimeSpan.Zero) return false;
        if (interval >= TimeSpan.FromDays(1)) return true;
        var untilNext = timeOfDay.TimeOfDay() - from.TimeOfDay();
        if (untilNext <= TimeSpan.Zero) untilNext += TimeSpan.FromDays(1);
        return untilNext <= interval;
    }
}
