using Tellurian.Trains.MeetingApp.Client.Model;

namespace Tellurian.Trains.MeetingApp.Client.Extensions;

public static class RegistrationExtensions
{
    public static bool IsWithUserNameAndPassword([NotNullWhen(true)] this Registration? me) =>
        me.IsWithUserName() && !string.IsNullOrEmpty(me?.ClockPassword);
    public static bool IsWithUserNameAndClockName([NotNullWhen(true)] this Registration? me) =>
        me.IsWithUserName() && !string.IsNullOrWhiteSpace(me?.ClockName);
    public static bool IsWithUserName([NotNullWhen(true)] this Registration? me) =>
        me is not null && me.UserName != ClockSettings.UnknownUserName && !string.IsNullOrWhiteSpace(me.UserName);
    public static bool IsAnalouge([NotNullWhen(true)] this Registration? me) =>
        me is not null && me.Display.Equals("Analogue", StringComparison.OrdinalIgnoreCase);
}
