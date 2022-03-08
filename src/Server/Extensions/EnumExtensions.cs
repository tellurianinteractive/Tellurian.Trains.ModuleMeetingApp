namespace Tellurian.Trains.MeetingApp.Server.Extensions;

using Tellurian.Trains.MeetingApp.Contracts.Models;

public static class EnumExtensions
{
    public static StopReason AsStopReason(this string? value) =>
        Enum.TryParse<StopReason>(value, out var stopReason) ? stopReason : StopReason.SelectStopReason;

    public static bool IsInvalid(this StopReason reason) => reason == StopReason.SelectStopReason;
}
