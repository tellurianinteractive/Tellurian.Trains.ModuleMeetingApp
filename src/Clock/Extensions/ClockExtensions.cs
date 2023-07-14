using Tellurian.Trains.MeetingApp.Contracts;
using Tellurian.Trains.MeetingApp.Contracts.Extensions;
using Tellurian.Trains.MeetingApp.Contracts.Models;

namespace Tellurian.Trains.MeetingApp.Clocks;

public static class ClockExtensions
{

    public static ClockStatus AsApiContract(this IClock me, IPAddress? remoteIpAddress, string? userName, string? clientVersion)
    {
        if (me is null) throw new ArgumentNullException(nameof(me));
        if (remoteIpAddress is not null && userName is not null) me.UpdateUser(remoteIpAddress, userName, clientVersion);
        return me.Status.AsApiContract();
    }

    public static ClockStatus AsApiContract(this Status me) =>
        me == null ? throw new ArgumentNullException(nameof(me)) :
        new ()
        {
            Duration = me.Duration.TotalHours,
            ExpectedResumeTimeAfterPause = me.ExpectedResumeTimeAfterPause.AsTimeOrEmpty(),
            FastEndTime = me.FastEndTime.AsTime(),
            IsCompleted = me.IsCompleted,
            IsPaused = me.IsPaused,
            IsRealtime = me.IsRealtime,
            IsRunning = me.IsRunning,
            IsElapsed = me.IsElapsed,
            Message = me.Message,
            Name = me.Name,
            PauseReason = me.PauseReason.ToString(),
            PauseTime = me.PauseTime.AsTimeOrEmpty(),
            RealEndTime = me.RealEndTime.AsTime(),
            Speed = me.Speed,
            StoppedByUser = me.StoppedByUser ?? "",
            StoppingReason = me.IsRunning ? string.Empty : me.StoppingReason.ToString(),
            Time = me.Time.AsTime(),
            Weekday = me.Weekday == Weekday.NoDay ? "" : me.Weekday.ToString()
        };


    public static IEnumerable<ClockUser> ClockUsers(this IClock me) => me is null ? Array.Empty<ClockUser>() : me.ClockUsers.OrderByDescending(u => u.LastUsedTime).Select(u => u.AsApiContract(me.UtcOffset));

    private static ClockUser AsApiContract(this Clocks.User me, TimeSpan timeZoneOffset) =>
        new ()
        {
            IPAddress = me.IPAddress.ToString(),
            UserName = me.UserName,
            ClientVersion = me.ClientVersion,
            LastUsedTime = me.LastUsedTime.ToOffset(timeZoneOffset).ToString("u", CultureInfo.InvariantCulture)
        };

    public static bool IsUser(this IClock me, string? password) =>
        me != null && me.IsUser(password);

    public static bool IsAdministrator(this IClock me, string? password) =>
        me != null && me.IsAdministrator(password);
}
