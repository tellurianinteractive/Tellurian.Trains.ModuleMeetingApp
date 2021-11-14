namespace Tellurian.Trains.MeetingApp.Clocks;

public class User : IEquatable<User>
{
    public User(IPAddress ipAddress, string? userName, string? clientVersion = null)
    {
        IPAddress = ipAddress;
        UserName = userName;
        ClientVersion = clientVersion;
    }
    public IPAddress IPAddress { get; }
    public string? UserName { get; private set; }
    public string? ClientVersion { get; private set; }
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
