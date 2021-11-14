namespace Tellurian.Trains.MeetingApp.Client;

public static class VersionExtensions
{
    public static string VersionNumber(this Version me) => me.ToString();
    public static string ComparableVersionNumber(this Version me) => $"{me.Major}.{me.Minor}";
    public static string ThreeDigitsVersionNumber(this Version me) => $"{me.Major}.{me.Minor}.{me.Build}";

}
