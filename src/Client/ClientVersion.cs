using System.Reflection;

namespace Tellurian.Trains.MeetingApp.Client;

public static class ClientVersion
{
    public static Version Value =>
        Assembly.GetExecutingAssembly()?.GetName().Version ?? new Version("0.0.0.0");

    public static bool IsCompatibleWithServerVersion(Version serverVersion) => 
        Value.ComparableVersionNumber() == serverVersion.ComparableVersionNumber();

    public static string VersionNumber => Value.ThreeDigitsVersionNumber();
}
