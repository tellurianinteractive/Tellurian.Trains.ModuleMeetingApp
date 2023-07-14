namespace Tellurian.Trains.MeetingApp.Contracts.Models;

public enum PauseReason
{
    NoReason,
    Breakfast,
    Lunch,
    Dinner,
    CoffeBreak,
    Meeting,
    DoneForToday,
    HallIsClosing,
}

public static class PauseReasonExtensions
{
    public static bool Is(this string value, PauseReason reason) =>
        reason.ToString().Equals(value, StringComparison.OrdinalIgnoreCase);
}
