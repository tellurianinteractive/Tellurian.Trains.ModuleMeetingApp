using System;
using System.Collections.Generic;
using System.Net;

namespace Tellurian.Trains.Clocks.Contracts
{
    public interface IClock
    {
        string Name { get; }
        TimeSpan UtcOffset { get; }
        DateTimeOffset LastUsedTime { get; }
        ClockSettings Settings { get; }
        ClockStatus Status { get; }
        IEnumerable<ClockUser> ClockUsers { get; }

        event EventHandler<string>? OnUpdate;

        bool UpdateUser(IPAddress? remoteIPAddress, string? userName, string? clientVersion);
        bool IsUser(string? password);
        bool IsAdministrator(string? password);
        bool TryStartTick(string? userName, string? password);
        bool TryStopTick(string? userName, string? password, StopReason reason);
        bool Update(string? userName, string? password, ClockSettings settings, IPAddress? remoteAddress);
    }
}
