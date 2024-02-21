using Tellurian.Trains.MeetingApp.Contracts;
using Tellurian.Trains.MeetingApp.Contracts.Extensions;
using Tellurian.Trains.MeetingApp.Contracts.Models;

namespace Tellurian.Trains.MeetingApp.Clocks;

public static class SettingsExtensions
{
    public static ClockSettings AsApiContract(this Settings me) =>
       me == null ? throw new ArgumentNullException(nameof(me)) :
       new ()
       {
           AdministratorPassword = me.AdministratorPassword ?? ClockSettings.DemoClockPassword,
           BreakTime = me.BreakTime.AsTimeOrEmpty(),
           DurationHours = me.Duration?.TotalHours,
           ExpectedResumeTime = me.ExpectedResumeTime.AsTimeOrEmpty(),
           IsElapsed = me.FastTime > me.StartTime,
           IsRunning = me.IsRunning,
           Message = me.Message?.DefaultText ?? string.Empty,
           Mode = me.IsRealtime ? "1" : "0",
           Name = me.Name,
           OverriddenElapsedTime = string.Empty,
           PauseReason = ((int)me.PauseReason).ToString(CultureInfo.CurrentCulture),
           PauseTime = me.PauseTime.AsTimeOrEmpty(),
           ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
           Speed = me.Speed,
           StartTime = me.StartTime.AsTimeOrEmpty(),
           StartWeekday = me.StartWeekday is null ? string.Empty : ((int)me.StartWeekday).ToString(CultureInfo.CurrentCulture),
           UserPassword = me.UserPassword ?? string.Empty
       };

    public static Settings AsSettings(this ClockSettings me) =>
        me == null ? throw new ArgumentNullException(nameof(me)) :
        new ()
        {
            AdministratorPassword = me.AdministratorPassword,
            BreakTime = me.BreakTime.AsTimeSpanOrNull(),
            Duration = me.DurationHours.AsTotalHours(),
            ExpectedResumeTime = me.ExpectedResumeTime.AsTimeSpanOrNull(),
            IsRealtime = me.Mode == "1",
            IsRunning = me.IsRunning,
            Message = new Message { DefaultText = me.Message ?? string.Empty },
            Name = me.Name,
            OverriddenElapsedTime = me.OverriddenElapsedTime.AsTimeSpanOrNull(),
            PauseReason = (PauseReason)int.Parse(me.PauseReason, CultureInfo.CurrentCulture),
            PauseTime = me.PauseTime.AsTimeSpanOrNull(),
            ShouldRestart = me.ShouldRestart,
            ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
            Speed = me.Speed,
            StartTime = me.StartTime.AsTimeSpanOrNull(),
            StartWeekday = (Weekday)int.Parse(me.StartWeekday, CultureInfo.CurrentCulture),
            UserPassword = me.UserPassword,
        };
}
