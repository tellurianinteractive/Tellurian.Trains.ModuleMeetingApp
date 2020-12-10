using System;
using System.Globalization;
using Tellurian.Trains.Clocks.Contracts;
using Tellurian.Trains.MeetingApp.Contract.Extensions;

namespace Tellurian.Trains.MeetingApp.Contract
{
    public static class ClockSettingsExtensions
    {
        public static ClockSettings AsApiContract(this Clocks.Contracts.ClockSettings me) =>
           me == null ? throw new ArgumentNullException(nameof(me)) :
           new ClockSettings
           {
               AdministratorPassword = me.AdministratorPassword ?? ClockSettings.DemoClockPassword,
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

        public static Clocks.Contracts.ClockSettings AsSettings(this ClockSettings me) =>
            me == null ? throw new ArgumentNullException(nameof(me)) :
            new Clocks.Contracts.ClockSettings
            {
                AdministratorPassword = me.AdministratorPassword,
                Duration = me.DurationHours.AsTotalHours(),
                ExpectedResumeTime = me.ExpectedResumeTime.AsTimeSpanOrNull(),
                IsRunning = me.IsRunning,
                Message = new ClockMessage { DefaultText = me.Message ?? string.Empty },
                Name = me.Name,
                OverriddenElapsedTime = me.OverriddenElapsedTime.AsTimeSpanOrNull(),
                PauseReason = (PauseReason)int.Parse(me.PauseReason, CultureInfo.CurrentCulture),
                PauseTime = me.PauseTime.AsTimeSpanOrNull(),
                ShouldRestart = me.ShouldRestart,
                ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
                Speed = me.Speed,
                StartTime = me.StartTime.AsTimeSpanOrNull(),
                StartWeekday = (Weekday)int.Parse(me.StartWeekday, CultureInfo.CurrentCulture),
                UserPassword = me.UserPassword
            };
    }
}