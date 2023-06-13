using System.Diagnostics.CodeAnalysis;
using Tellurian.Trains.MeetingApp.Client.Model;
using Tellurian.Trains.MeetingApp.Contracts.Extensions;


namespace Tellurian.Trains.MeetingApp.Client.Extensions;

public static class ClockStatusExtensions
{
    public static string StatusClass(this ClockStatus me)
    {
        if (me?.IsUnavailable != false) return "unavailable";
        if (me.IsRealtime) return "realtime";
        if (me.IsRunning) return "fastclock";
        return "stopped";
    }

    private static double MinutesPerGameHour(this ClockStatus me) => 60 / (me.Speed < 0 ? 1 : me.Speed);
    public static double SecondsPerGameMinute(this ClockStatus? me) => 60 / (me is null || me.Speed < 0 || me.IsRealtime ? 1 : me.Speed);
    public static double MinutesPerHour(this ClockStatus me) => me.IsRealtime ? 60 : Math.Floor(me.MinutesPerGameHour());
    public static double SecondsReminderPerHour(this ClockStatus me)
    {
        var gameHour = me.MinutesPerGameHour();
        return (gameHour - Math.Floor(gameHour)) * 60;
    }

    public static double Hours(this ClockStatus? me) => me.HasTime() ? double.Parse(me.Time[..2]) + me.Minutes() / 60.0 : 0;
    public static int Minutes(this ClockStatus? me) => me.HasTime() ? int.Parse(me.Time[3..]) : 0;
    private static bool HasTime([NotNullWhen(true)] this ClockStatus? me) => me is not null && me.Time.Length == 5;

    public static bool IsClientVersionCompatibleWithServerVersion(this ClockStatus? me) =>
        me is null || me.ServerVersionNumber.StartsWith(ClientVersion.Value.ComparableVersionNumber());

    public static string TimeFontSize(this ClockStatus? me, Registration? registration) =>
        me.HasMessageText(40) ? "24vw" :
        me.HasMessageText(20) ? "28vw" :
        registration?.DisplayTimeMaximized == false ? "32vw" :
        "35vw";
    public static bool HasMessageText(this ClockStatus? me, int minLength = 1)
        => me is not null && (me.Message.Length >= minLength || me.IsPaused || me.IsUnavailable || (me.StoppedByUser.HasValue() && me.StoppingReason != nameof(Contracts.Models.StopReason.Other)));
}
