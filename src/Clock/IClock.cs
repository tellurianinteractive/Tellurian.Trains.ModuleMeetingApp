using System.Net;
using Tellurian.Trains.MeetingApp.Contracts.Models;

namespace Tellurian.Trains.MeetingApp.Clocks;

public interface IClock
{
    string Name { get; }
    TimeSpan UtcOffset { get; }
    DateTimeOffset LastAccessedTime { get; }
    Settings Settings { get; }
    Status Status { get; }
    IEnumerable<User> ClockUsers { get; }

    event EventHandler<string>? OnUpdate;

    bool UpdateSettings(IPAddress? ipAddress, string? username, string? password, Settings settings);
    bool UpdateUser(IPAddress? ipAddress, string? username, string? clientVersion = "");
    bool IsUser(string? password);
    bool IsAdministrator(string? password);
    bool TryStartTick(string? userName, string? password);
    bool TryStopTick(string? userName, string? password, StopReason reason);
}
