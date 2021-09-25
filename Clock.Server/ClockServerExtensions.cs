using System;
using System.Runtime.CompilerServices;
using Tellurian.Trains.Clocks.Contracts;

[assembly: InternalsVisibleTo("Tellurian.Trains.Clocks.Server.Tests")]

namespace Tellurian.Trains.Clocks.Server
{
    internal static class ClockServerExtensions
    {
        public static int WeekdayNumber(this TimeSpan me) =>
            me.Days == 0 ? 0 :
            ((me.Days - 1) % 7) + 1;

        public static ClockSettings AsSettings(this ClockServerOptions me) =>
            me == null ? throw new ArgumentNullException(nameof(me)) :
            new ClockSettings
            {
                Name = me.Name,
                AdministratorPassword = me.Password,
                Duration = me.Duration,
                StartTime = me.StartTime,
                Speed = me.Speed
            };

        public static ClockSettings AsSettings(this ClockServer me) =>
            new ()
            {
                AdministratorPassword = me.AdministratorPassword,
                Duration = me.Duration,
                ExpectedResumeTime = me.ExpectedResumeTime,
                IsRealtime = me.IsRealtime,
                IsRunning = me.IsRunning,
                FastTime = me.FastTime,
                Message = me.Message,
                Name = me.Name,
                PauseReason = me.PauseReason,
                PauseTime = me.PauseTime,
                ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
                Speed = me.Speed,
                StartTime = me.StartTime,
                StartWeekday = me.Weekday,
                TimeZoneOffset = me.UtcOffset,
                UserPassword = me.UserPassword
            };

        public static ClockStatus AsStatus(this ClockServer me) =>
            new()
            {
                Duration = me.Duration,
                ExpectedResumeTimeAfterPause = me.ExpectedResumeTime,
                FastEndTime = me.FastEndTime,
                IsCompleted = me.IsCompleted,
                IsPaused = me.IsPaused,
                IsRealtime = me.IsRealtime,
                IsRunning = me.IsRunning,
                Message = me.Message.DefaultText ?? "",
                Name = me.Name,
                PauseReason = me.PauseReason,
                PauseTime = me.PauseTime,
                RealEndTime = me.RealEndTime,
                Speed = me.Speed,
                StoppedByUser = me.StoppingUser ?? "",
                StoppingReason = me.StopReason,
                Time = me.Time,
                Weekday = me.Weekday
            };
    }
}
