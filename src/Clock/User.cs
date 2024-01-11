namespace Tellurian.Trains.MeetingApp.Clocks;

public class User(IPAddress ipAddress, string? userName, string? clientVersion = null) : IEquatable<User>
{
    public IPAddress IPAddress { get; } = ipAddress;
    public string? UserName { get; private set; } = userName;
    public string? ClientVersion { get; private set; } = clientVersion;
    public DateTimeOffset LastUsedTime { get; private set; } = DateTimeOffset.Now;
    public void Update(string? userName, string? clientVersion = null)
    {
        LastUsedTime = DateTimeOffset.Now;
        UserName = userName;
        ClientVersion = clientVersion;
    }
    public override string ToString() => $"{UserName ?? "Unknown"}@{IPAddress} {LastUsedTime}";
    public override bool Equals(object? obj) => obj is User other && Equals(other);
    public bool Equals(User? other) => other is not null && other.IPAddress == IPAddress;
    public override int GetHashCode() => UserName is null ? IPAddress.GetHashCode() : HashCode.Combine(UserName, IPAddress);
}
