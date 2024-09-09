using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

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

    public static string MinutesPerHour(this double minutesPerHour) =>
        minutesPerHour > 10 ? "" :
        minutesPerHour > 9.75 ? "10" :
        minutesPerHour > 9.25 ? "9½" :
        minutesPerHour > 8.75 ? "9" :
        minutesPerHour > 8.25 ? "9½" :
        minutesPerHour > 7.75 ? "8" :
        minutesPerHour > 7.25 ? "7½" :
        minutesPerHour > 6.75 ? "7" :
        minutesPerHour > 6.25 ? "6½" :
        minutesPerHour > 5.75 ? "6" :
        minutesPerHour > 5.25 ? "5½" :
        minutesPerHour > 4.75 ? "5" :
        minutesPerHour > 4.25 ? "4½" :
        minutesPerHour > 3.75 ? "4" :
        minutesPerHour > 3.25 ? "3½" :
        minutesPerHour > 2.75 ? "3" :
        minutesPerHour > 2.25 ? "2½" :
        minutesPerHour > 1.75 ? "2" :
        minutesPerHour > 1.25 ? "1½" :
        minutesPerHour > 0.75 ? "1" :
        "";





    public static double SecondsReminderPerHour(this ClockStatus me)
    {
        var gameHour = me.MinutesPerGameHour();
        return (gameHour - Math.Floor(gameHour)) * 60;
    }

    public static double Hours(this ClockStatus? me) => me.HasTime() ? double.Parse(me.Time[..2]) + me.Minutes() / 60.0 : 0;
    public static int Minutes(this ClockStatus? me) => me.HasTime() ? int.Parse(me.Time[3..]) : 0;
    private static bool HasTime([NotNullWhen(true)] this ClockStatus? me) => me is not null && me.Time.Length == 5;

    public static bool IsClientVersionCompatibleWithServerVersion(this ClockStatus? me) =>
        me is null || string.IsNullOrWhiteSpace(me.ServerVersionNumber) || me.ServerVersionNumber.StartsWith(ClientVersion.Value.ComparableVersionNumber());

    public static string TimeFontSize(this ClockStatus? me, Registration? registration) =>
        me.HasMessageText(40) ? "24vw" :
        me.HasMessageText(20) ? "28vw" :
        me.ShowClockQrCode() ? "24vw":
        registration?.DisplayTimeMaximized == false ? "32vw" :
        "35vw";
    public static bool HasMessageText([NotNullWhen(true)] this ClockStatus? me, int minLength = 1)
        => me is not null && (me.Message.Length >= minLength || me.IsPaused || me.IsUnavailable || (me.StoppedByUser.HasValue() && me.StoppingReason != nameof(Contracts.Models.StopReason.Other)));

    public static bool ShowClockQrCode(this ClockStatus? me) => me?.IsRunning == false && me?.IsElapsed == false;
    public static string ClockUrl(this ClockStatus? me, NavigationManager navigator) =>
        me is null ? string.Empty :
            $"{navigator.BaseUri.Replace("localhost", me.HostAddress)}clock/{me.Name}";


    public static string PauseMessage(this ClockStatus me, IStringLocalizer<App> localizer) =>
        me.PauseReason.EqualsCaseInsensitive(PauseReason.NoReason.ToString()) ? string.Empty :
        me.PauseReason.EqualsCaseInsensitive(PauseReason.DoneForToday.ToString()) ? $"{localizer[me.PauseReason]}." :
        $"{localizer["GameIsPausedFor"]} {localizer[me.PauseReason]}.";

    public static string PauseStatus(this ClockStatus me, IStringLocalizer<App> localizer) =>
        me.PauseTime.HasValue() ?
            me.ExpectedResumeTimeAfterPause.HasValue() ? string.Format(CultureInfo.CurrentCulture, localizer["PauseBetweenTimeForReason"].Value, me.PauseTime, me.ExpectedResumeTimeAfterPause, localizer[me.PauseReason]) :
            me.PauseReason.Is(PauseReason.DoneForToday) ? string.Format(CultureInfo.CurrentCulture, localizer["StopsAtTimeForReason"].Value, me.PauseTime, localizer[me.PauseReason]) :
            me.PauseReason.Is(PauseReason.HallIsClosing) ? string.Format(CultureInfo.CurrentCulture, localizer["StopsAtTimeForReason"].Value, me.PauseTime, localizer[me.PauseReason]) :
            string.Format(CultureInfo.CurrentCulture, localizer["PauseAtTimeForReason"].Value, me.PauseTime, localizer[me.PauseReason]) :
        string.Empty;

    public static string StoppingMessage(this ClockStatus? me, IStringLocalizer<App> localizer) =>
        me is null ? string.Empty :
        me.StoppingReason == nameof(StopReason.Other) ? string.Format(localizer["StoppedByUser"].Value, me.StoppedByUser) :
        string.Format(localizer["UserHasStoppedOfReason"].Value, me.StoppedByUser, localizer[me.StoppingReason]);

    public static bool ShowStoppingMessage(this ClockStatus? me) =>
        me is not null && !me.IsRealtime && me.StoppedByUser.HasValue() && me.StoppingReason.HasValue();

    public static string GameEndTimeStatus(this ClockStatus? me, IStringLocalizer<App> localizer) =>
        me is null ? string.Empty :
        me.PauseTime.HasValue() && me.PauseReason.Is(PauseReason.DoneForToday) || me.PauseReason.Is(PauseReason.HallIsClosing) ? string.Empty :
        string.Format(CultureInfo.CurrentCulture, localizer["GameEndsAtTime"].Value, me.FastEndTime, me.RealEndTime);
}




