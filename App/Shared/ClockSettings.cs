using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Tellurian.Trains.Clocks.Server;
using Tellurian.Trains.MeetingApp.Shared.Resources;

namespace Tellurian.Trains.MeetingApp.Shared
{
    public class ClockSettings
    {
        public static string DemoClockName => Clocks.Server.ClockSettings.DefaultName;
        public static string DemoClockPassword => Clocks.Server.ClockSettings.DefaultPassword;
        /// <summary>
        /// Name of clock. If a non-extisting clock name is given, a new clock instance is created.
        /// </summary>
        [Display(Name = "ClockName", ResourceType = typeof(Strings))]
        [Required( ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
        public string? Name { get; set; } = DemoClockName;
        /// <summary>
        /// True if clock should resume game from start time.
        /// </summary>
        [Display(Name = nameof(ShouldRestart), ResourceType = typeof(Strings))]
        public bool ShouldRestart { get; set; }
        /// <summary>
        /// True if current game time is later than game start time.
        /// </summary>
        [Display(Name = nameof(IsElapsed), ResourceType = typeof(Strings))]
        public bool IsElapsed { get; set; }
        /// <summary>
        /// True if clock is running (or should be running).
        /// </summary>
        [Display(Name = nameof(IsRunning), ResourceType = typeof(Strings))]
        public bool IsRunning { get; set; }
        /// <summary>
        /// The weekday that the game should start at. Weekdays are defined in <see cref="Weekday"/>.
        /// </summary>
        [Display(Name = nameof(StartWeekday), ResourceType = typeof(Strings))]
        public string StartWeekday { get; set; } = "0";
        /// <summary>
        /// The game start time.
        /// </summary>
        [Display(Name = nameof(StartTime), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
        [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
        public string StartTime { get; set; } = string.Empty;
        /// <summary>
        /// The game time speed. Decimal value.
        /// </summary>
        [Display(Name = nameof(Speed), ResourceType = typeof(Strings))]
        [Range(1.0, 7.0, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(Strings))]
        public double? Speed { get; set; } = 6;
        /// <summary>
        /// Duration of the game in hours (with decimals).
        /// </summary>
        [Display(Name = nameof(DurationHours), ResourceType = typeof(Strings))]
        [Range(1.0, 168.0, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(Strings))]
        public double? DurationHours { get; set; } = 12;
        /// <summary>
        /// Real time when pause starts.
        /// </summary>
        [Display(Name = nameof(PauseTime), ResourceType = typeof(Strings))]
        [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
        public string PauseTime { get; set; } = string.Empty;
        /// <summary>
        /// Real time when pause starts.
        /// </summary>
        [Display(Name = nameof(PauseReason), ResourceType = typeof(Strings))]
        public string PauseReason { get; set; } = "0";
        /// <summary>
        /// Expected real time when game will resume or empty.
        /// </summary>
        [Display(Name = nameof(ExpectedResumeTime), ResourceType = typeof(Strings))]
        [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
        public string ExpectedResumeTime { get; set; } = string.Empty;
        /// <summary>
        /// True if real time should be shown during pause.
        /// </summary>
        [Display(Name = nameof(ShowRealTimeWhenPaused), ResourceType = typeof(Strings))]
        public bool ShowRealTimeWhenPaused { get; set; }
        /// <summary>
        /// Option to change the current elapsed game time, for example if clock is stopped to late and game time have to be set back in time.
        /// </summary>
        [Display(Name = nameof(OverriddenElapsedTime), ResourceType = typeof(Strings))]
        [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
        public string OverriddenElapsedTime { get; set; } = string.Empty;
        /// <summary>
        /// Eventually manual entered message by the administrator to display.
        /// </summary>
        [Display(Name = nameof(Message), ResourceType = typeof(Strings))]
        [StringLength(50, ErrorMessageResourceName = "InvalidString", ErrorMessageResourceType = typeof(Strings))]
        public string Message { get; set; } = string.Empty;
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
        public string Mode { get; set; } = "0";
        /// <summary>
        /// Clock administrator password.
        /// </summary>
        [Display(Name = nameof(AdministratorPassword), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
        [StringLength(10, ErrorMessageResourceName = "InvalidString", ErrorMessageResourceType = typeof(Strings))]
        public string AdministratorPassword { get; set; } = DemoClockPassword;
        /// <summary>
        /// User administrator password.
        /// </summary>
        [Display(Name = nameof(UserPassword), ResourceType = typeof(Strings))]
        [StringLength(10, ErrorMessageResourceName = "InvalidString", ErrorMessageResourceType = typeof(Strings))]
        public string UserPassword { get; set; } = string.Empty;
    }

    public static class ClockSettingsExtensions
    {
        public static Clocks.Server.ClockSettings AsSettings(this ClockSettings me) =>
            me == null ? throw new ArgumentNullException(nameof(me)) :
            new Clocks.Server.ClockSettings
            {
                AdministratorPassword = me.AdministratorPassword,
                DurationHours = me.DurationHours,
                ExpectedResumeTime = me.ExpectedResumeTime.AsTimeSpanOrNull(),
                IsRealTime = me.Mode == "1",
                IsRunning = me.IsRunning,
                Message = new ClockMessage { DefaultText = me.Message },
                Name = me.Name,
                OverriddenElapsedTime = me.OverriddenElapsedTime.AsTimeSpanOrNull(),
                PauseReason = (PauseReason)int.Parse(me.PauseReason, CultureInfo.CurrentCulture),
                PauseTime = me.PauseTime.AsTimeSpanOrNull(),
                ShouldRestart = me.ShouldRestart,
                ShowRealTimeWhenPaused = me.ShowRealTimeWhenPaused,
                Speed = me.Speed,
                StartTime = me.StartTime.AsTimeSpanOrNull(),
                StartWeekday = (Weekday)int.Parse(me.StartWeekday, CultureInfo.CurrentCulture),
                UserPassword = me.UserPassword
            };
    }
}