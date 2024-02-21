using System.Reflection;

namespace Tellurian.Trains.MeetingApp.Client;

public static class ClientVersion
{
    public static Version Value =>
        Assembly.GetExecutingAssembly()?.GetName().Version ?? new Version("0.0.0.0");

    public static bool IsCompatibleWithServerVersion(this string serverVersion) => 
        VersionNumber.StartsWith(serverVersion);

    public static string VersionNumber => Value.ThreeDigitsVersionNumber();
}
