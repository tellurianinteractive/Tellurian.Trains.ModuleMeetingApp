using System;
using System.Globalization;
using Tellurian.Trains.Clocks.Server;

namespace Tellurian.Trains.MeetingApp.Shared
{
    public class ClockSettings
    {
        public static string DemoClockName => Clocks.Server.ClockSettings.DefaultName;
        public static string DemoClockPassword => Clocks.Server.ClockSettings.DefaultPassword;
        public static string ClockApiKey => "7EA656FB-34E1-48BE-8E16-296170A0E883";
        public string? Name { get; set; } = DemoClockName;
        public bool ShouldRestart { get; set; }
        public bool IsRunning { get; set; }
        public string StartWeekday { get; set; } = "0";
        public string StartTime { get; set; } = string.Empty;
        public double? Speed { get; set; } = 6;
        public double? DurationHours { get; set; } = 12;
        public string PauseTime { get; set; } = string.Empty;
        public string PauseReason { get; set; } = "0";
        public string ExpectedResumeTime { get; set; } = string.Empty;
        public bool ShowRealTimeWhenPaused { get; set; }
        public string OverriddenElapsedTime { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Mode { get; set; } = "0";
        public string Password { get; set; } = DemoClockPassword;
    }

    public static class ClockSettingsExtensions
    {
        public static Clocks.Server.ClockSettings AsSettings(this ClockSettings me) =>
            me == null ? throw new ArgumentNullException(nameof(me)) :
            new Clocks.Server.ClockSettings
            {
                DurationHours = me.DurationHours,
                ExpectedResumeTime = me.ExpectedResumeTime.AsTimeSpanOrNull(),
                IsRealTime = me.Mode == "1",
                IsRunning = me.IsRunning,
                Message = new ClockMessage { DefaultText = me.Message },
                Name = me.Name,
                Password = me.Password,
                OverriddenElapsedTime = me.OverriddenElapsedTime.AsTimeSpanOrNull(),
                PauseReason = (PauseReason)(int.Parse(me.PauseReason, CultureInfo.CurrentCulture)),
                PauseTime = me.PauseTime.AsTimeSpanOrNull(),
                ShouldRestart = me.ShouldRestart,
                ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
                Speed = me.Speed,
                StartTime = me.StartTime.AsTimeSpanOrNull(),
                StartWeekday = (Weekday)(int.Parse(me.StartWeekday, CultureInfo.CurrentCulture))
            };
    }
}