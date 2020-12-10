using System;
using System.Reflection;
using Tellurian.Trains.MeetingApp.Contract;

namespace Tellurian.Trains.MeetingApp.Client
{
    public static class Client
    {
        public static string VersionNumber => ClientVersion.ToString();
        private static Version ClientVersion => Assembly.GetExecutingAssembly()?.GetName().Version ?? new Version("0.0.0.0");

    }
    public static class ClockStatusExtensions
    {
        public static bool IsClientVersionSameAsServer(this ClockStatus me) => me?.ServerVersionNumber.StartsWith(ClientVersion.ComparableVersionNumber(), StringComparison.Ordinal) == true;
        private static Version ClientVersion => Assembly.GetExecutingAssembly()?.GetName().Version ?? new Version("0.0.0.0");
        private static string ComparableVersionNumber(this Version me) => $"{me.Major}.{me.Minor}";
    }
}
