using System.Reflection;

namespace Tellurian.Trains.MeetingApp.Server;

public static class AppVersion
{
    public static Version? ServerVersion => Assembly.GetAssembly(typeof(AppVersion)).GetName().Version;

}
