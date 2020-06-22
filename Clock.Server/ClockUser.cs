using System;
using System.Net;

namespace Tellurian.Trains.Clocks.Server
{
    public class ClockUser : IEquatable<ClockUser>
    {
        public ClockUser(IPAddress ipAddress, string? userName)
        {
            IPAddress = ipAddress;
            UserName = userName;
        }
        public IPAddress IPAddress { get; }
        public string? UserName { get; private set; }
        public DateTimeOffset LastUsedTime { get; private set; } = DateTimeOffset.Now;
        public void Update(string? userName) { LastUsedTime = DateTimeOffset.Now; UserName = string.IsNullOrWhiteSpace(userName) ? "Unknown" : userName; }
        public override string ToString() => $"{UserName ?? "Unknown"}@{IPAddress} {LastUsedTime}";
        public override bool Equals(object obj) => obj is ClockUser other && Equals(other);
        public bool Equals(ClockUser other) => !(other is null) && other.IPAddress == IPAddress;
        public override int GetHashCode() => UserName is null ? IPAddress.GetHashCode() : HashCode.Combine(UserName, IPAddress);
    }
}
