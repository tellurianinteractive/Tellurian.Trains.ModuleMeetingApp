using Tellurian.Trains.MeetingApp.Client.Model;

namespace Tellurian.Trains.MeetingApp.Client.Extensions;

public static class RegistrationExtensions
{
    public static bool IsWithUserNameAndPassword(this Registration? me) =>
        me.IsWithUserName() && !string.IsNullOrEmpty(me?.ClockPassword);
    public static bool IsWithUserNameAndClockName(this Registration? me) =>
        me.IsWithUserName() && !string.IsNullOrWhiteSpace(me?.ClockName);
    public static bool IsWithUserName(this Registration? me) =>
        me is not null && !string.IsNullOrWhiteSpace(me.UserName);
    public static bool IsAnalouge(this Registration? me) =>
        me is not null && me.Display.Equals("Analogue", StringComparison.OrdinalIgnoreCase);
}
