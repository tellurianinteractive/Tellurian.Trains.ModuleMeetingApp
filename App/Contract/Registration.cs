namespace Tellurian.Trains.MeetingApp.Contract
{
    public class Registration
    {
        public string? UserName { get; set; }
        public string? ClockName { get; set; }= ClockSettings.DemoClockName;
        public string? ClockPassword { get; set; } = ClockSettings.DemoClockPassword;
        public string Theme { get; set; } = "Dark";
        public bool IsInstructionVisible { get; set; } = true;
        public bool DisplayTimeMaximized { get; set; }

        public const string Key = "registration";
    }

    public enum Theme
    {
        Light,
        Dark
    }

    public static class RegistrationExtensions
    {
        public static bool IsWithUserNameAndPassword(this Registration? me) => me.IsWithUserName() && !string.IsNullOrEmpty(me?.ClockPassword);
        public static bool IsWithUserNameAndClockName(this Registration? me) => me.IsWithUserName() && !string.IsNullOrWhiteSpace(me?.ClockName);
        public static bool IsWithUserName(this Registration? me) => me != null && !string.IsNullOrWhiteSpace(me.UserName);
    }
}
