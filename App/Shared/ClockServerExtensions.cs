using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Tellurian.Trains.Clocks.Server;

namespace Tellurian.Trains.MeetingApp.Shared
{
    public static class ClockServerExtensions
    {
        public static ClockSettings GetSettings(this ClockServer me) =>
       me == null ? throw new ArgumentNullException(nameof(me)) :
       new ClockSettings
       {
           AdministratorPassword = me.AdministratorPassword,
           DurationHours = me.Duration.Hours,
           ExpectedResumeTime = me.ExpectedResumeTime.AsTimeOrEmpty(),
           IsElapsed = me.FastTime > me.StartTime,
           IsRunning = me.IsRunning,
           Message = me.Message.DefaultText,
           Mode = me.IsRealtime ? "1" : "0",
           Name = me.Name,
           OverriddenElapsedTime = string.Empty,
           PauseReason = ((int)me.PauseReason).ToString(CultureInfo.CurrentCulture),
           PauseTime = me.PauseTime.AsTimeOrEmpty(),
           ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
           Speed = me.Speed,
           StartTime = me.StartTime.AsTime(),
           StartWeekday = ((int)me.Weekday).ToString(CultureInfo.CurrentCulture),
           UserPassword = me.UserPassword
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

        public static ClockStatus GetStatus(this ClockServer me, IPAddress remoteIpAddress, string? userName)
        {
            if (me is null) throw new ArgumentNullException(nameof(me));
            me.UpdateUser(remoteIpAddress, userName);
            return me.GetStatus();
        }

        public static IEnumerable<ClockUser> ClockUsers(this ClockServer me) => me is null ? Array.Empty<ClockUser>() : me.ClockUsers.OrderByDescending(u => u.LastUsedTime).Select(u => u.AsClockUser(me.UtcOffset));

        private static ClockUser AsClockUser(this Clocks.Server.ClockUser me, TimeSpan timeZoneOffset) =>
            new ClockUser()
            {
                IPAddress = me.IPAddress.ToString(),
                UserName = me.UserName,
                ClientVersion = me.ClientVersion,
                LastUsedTime = me.LastUsedTime.ToOffset(timeZoneOffset).ToString("u", CultureInfo.InvariantCulture)
            };

        public static bool IsUser(this ClockServer me, string? password) =>
            me != null &&
            (string.IsNullOrEmpty(me.UserPassword) ||
                me.UserPassword.Equals(password, StringComparison.Ordinal) ||
                me.AdministratorPassword.Equals(password, StringComparison.Ordinal));

        public static bool IsAdministrator(this ClockServer me, string? password) =>
            me?.AdministratorPassword.Equals(password, StringComparison.Ordinal) == true;
    }
}
