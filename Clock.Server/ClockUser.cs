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
        public string? UserName { get; }
        public DateTimeOffset LastUsedTime { get; private set; } = DateTimeOffset.Now;
        public void UsedNow() => LastUsedTime = DateTimeOffset.Now;
        public override string ToString() => $"{UserName ?? "Unknown"}@{IPAddress} {LastUsedTime}";
        public override bool Equals(object obj) => obj is ClockUser;

        public bool Equals(ClockUser other) =>
            !(other is null) && (UserName is null ? other.IPAddress == IPAddress :
            other.UserName == UserName && other.IPAddress == IPAddress);

        public bool Is(IPAddress iPAddress, string? userName) =>
            iPAddress == IPAddress && (userName?.Equals(UserName, StringComparison.OrdinalIgnoreCase) != false);

        public override int GetHashCode() => UserName is null ? IPAddress.GetHashCode() : HashCode.Combine(UserName, IPAddress);
    }
}
