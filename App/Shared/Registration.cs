namespace Tellurian.Trains.MeetingApp.Shared
{
    public class Registration
    {
        public string? Name { get; set; } 
        public string? Password { get; set; }

        public const string Key = "registration";
    }

    public static class RegistrationExtensions
    {
        public static bool IsRegistered(this Registration me) => !string.IsNullOrEmpty(me?.Name);
    }
}
