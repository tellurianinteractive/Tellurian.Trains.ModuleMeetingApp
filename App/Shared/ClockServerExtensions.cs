using System;
using System.Globalization;
using Tellurian.Trains.Clocks.Server;

#pragma warning disable CA1716

namespace Tellurian.Trains.MeetingApp.Shared
{
    public static class ClockServerExtensions
    {
        public static ClockSettings GetSettings(this ClockServer me) =>
       me == null ? throw new ArgumentNullException(nameof(me)) :
       new ClockSettings
       {
           DurationHours = me.Duration.Hours,
           ExpectedResumeTime = me.ExpectedResumeTime.AsTimeOrEmpty(),
           IsRunning = me.IsRunning,
           Message = me.Message.DefaultText,
           Mode = me.IsRealtime ? "1" : "0",
           Name = me.Name,
           OverriddenElapsedTime = string.Empty,
           Password = me.Password,
           PauseReason = ((int)me.PauseReason).ToString(CultureInfo.CurrentCulture),
           PauseTime = me.PauseTime.AsTimeOrEmpty(),
           ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
           Speed = me.Speed,
           StartTime = me.StartTime.AsTime(),
           StartWeekday = ((int)me.Weekday).ToString(CultureInfo.CurrentCulture)
       };

        public static ClockStatus GetStatus(this ClockServer me) =>
            me == null ? throw new ArgumentNullException(nameof(me)) :
            new ClockStatus
            {
                Duration = me.Duration.TotalHours,
                ExpectedResumeTimeAfterPause = me.ExpectedResumeTime.AsTimeOrEmpty(),
                FastEndTime = me.FastEndTime.AsTime(),
                IsCompleted = me.IsCompleted,
                IsPaused = me.IsPaused,
                IsRealtime = me.IsRealtime,
                IsRunning = me.IsRunning,
                Message = me.Message.DefaultText ?? "",
                Name = me.Name,
                PauseReason = me.PauseReason.ToString(),
                PauseTime = me.PauseTime.AsTimeOrEmpty(),
                RealEndTime = me.RealEndTime.AsTime(),
                Speed = me.Speed,
                StoppedByUser = me.StoppingUser ?? "",
                StoppingReason = me.IsRunning ? string.Empty : me.StopReason.ToString(),
                Time = me.Time.AsTime(),
                Weekday = me.Weekday == Weekday.NoDay ? "" : me.Weekday.ToString()
            };
    }
}
