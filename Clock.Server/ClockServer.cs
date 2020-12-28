using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;
using Tellurian.Trains.Clocks.Contracts;
using Tellurian.Trains.Clocks.Server.Integrations;

[assembly: InternalsVisibleTo("Tellurian.Trains.Clocks.Server.Tests")]

namespace Tellurian.Trains.Clocks.Server
{
    public sealed class ClockServer : IDisposable, IClock
    {
        private readonly string NewLine = Environment.NewLine;

        private readonly ClockServerOptions Options;
        private readonly Timer ClockTimer;
        private readonly ClockMulticaster Multicaster;
        private readonly ClockPollingService PollingService;
        private readonly IList<ClockUser> Clients = new List<ClockUser>();
        public event EventHandler<string>? OnUpdate;

        public static Version? ServerVersion => Assembly.GetAssembly(typeof(ClockServer))?.GetName().Version;

        public ClockServer(IOptions<ClockServerOptions> options)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            Name = Options.Name;
            AdministratorPassword = Options.Password;
            UserPassword = string.Empty;
            Update(Options.AsSettings());
            Elapsed = TimeSpan.Zero;
            ClockTimer = new Timer(1000);
            ClockTimer.Elapsed += Tick;
            Multicaster = new ClockMulticaster(Options.Multicast, this);
            PollingService = new ClockPollingService(Options.Polling, this);
            UtcOffset = TimeZoneInfo.FindSystemTimeZoneById(Options.TimeZoneId).GetUtcOffset(DateTime.Today);
        }
        public string Name { get; internal set; }
        public TimeSpan UtcOffset { get; }
        public ClockSettings Settings { get => this.AsSettings(); set => Update(value); }
        public ClockStatus Status => this.AsStatus();
        public IEnumerable<ClockUser> ClockUsers => Clients.ToArray();
        public DateTimeOffset LastUsedTime => Clients.Count > 0 ? Clients.Max(c => c.LastUsedTime) : DateTimeOffset.Now;

        public bool IsUser(string? password) =>
            string.IsNullOrWhiteSpace(UserPassword) && string.IsNullOrWhiteSpace(password) ||
            IsAdministrator(password) ||
            (!string.IsNullOrWhiteSpace(password) && password.Equals(UserPassword, StringComparison.OrdinalIgnoreCase));

        private bool IsStoppingUser(string? userName, string? password) =>
            IsUser(password) &&
            !string.IsNullOrWhiteSpace(userName) && userName.Equals(StoppingUser, StringComparison.OrdinalIgnoreCase);

        public bool IsAdministrator(string? password) =>
            !string.IsNullOrWhiteSpace(password) && password.Equals(AdministratorPassword, StringComparison.OrdinalIgnoreCase);

        internal bool IsPaused { get; private set; }
        internal bool IsRunning { get; private set; }
        internal bool IsRealtime { get; private set; }
        internal bool IsCompleted => Elapsed >= Duration;
        internal ClockMessage Message { get; private set; } = new ClockMessage();
        internal bool ShowRealTimeWhenPaused { get; private set; }
        internal double Speed { get; private set; }
        internal PauseReason PauseReason { get; private set; }
        internal StopReason StopReason { get; private set; }
        internal string? StoppingUser { get; private set; }
        internal Weekday Weekday => IsRealtime ? (Weekday)RealDayAndTime.WeekdayNumber() : (Weekday)FastTime.WeekdayNumber();
        internal string AdministratorPassword { get; set; }
        internal string UserPassword { get; private set; }

        internal TimeSpan StartDayAndTime { get; private set; }
        internal TimeSpan StartTime => StartDayAndTime - TimeSpan.FromDays(StartDayAndTime.Days);
        internal TimeSpan Duration { get; private set; }
        internal TimeSpan Elapsed { get; private set; }
        internal TimeSpan FastTime => StartDayAndTime + Elapsed;
        internal TimeSpan Time { get { return IsRealtime ? RealDayAndTime : FastTime; } }
        internal TimeSpan RealEndTime => RealDayAndTime + TimeSpan.FromHours((Duration - Elapsed).TotalHours / Speed) + PauseDuration;
        internal TimeSpan FastEndTime => StartDayAndTime + Duration;
        internal TimeSpan? PauseTime { get; private set; }
        internal TimeSpan? ExpectedResumeTime { get; private set; }
        internal TimeSpan PauseDuration => ExpectedResumeTime.HasValue && PauseTime.HasValue ? ExpectedResumeTime.Value - PauseTime.Value : TimeSpan.Zero;
        internal TimeSpan RealDayAndTime { get { var now = DateTime.UtcNow + UtcOffset; var day = (int)now.DayOfWeek; return new TimeSpan(day == 0 ? 7 : day, now.Hour, now.Minute, now.Second); } }
        internal TimeSpan RealTime => RealDayAndTime - TimeSpan.FromDays(RealDayAndTime.Days);
        public override string ToString() => Name;

        #region Clock control
        public ClockServer StartServer(ClockSettings settings)
        {
            Update(settings);
            PollingService.TryStartPolling();
            Multicaster.TryStartMulticast();
            return this;
        }

        public void StopServer()
        {
            Multicaster.StopMulticast();
            PollingService.StopPolling();
            ClockTimer.Stop();
        }

        public bool TryStartTick(string? user, string? password)
        {
            if (IsRunning) return true;
            if (IsStoppingUser(user, password) || IsAdministrator(password))
            {
                if (IsAdministrator(password)) ResetPause();
                ResetStopping();
                ClockTimer.Start();
                IsRunning = true;
                if (Options.Sounds.PlayAnnouncements) PlaySound(Options.Sounds.StartSoundFilePath);
                return true;
            }
            return false;
        }

        public bool TryStopTick(string? user, string? password, StopReason reason)
        {
            if (IsRunning && IsUser(password) && !string.IsNullOrWhiteSpace(user))
            {
                StopTick(reason, user);
                return true;
            }
            return false;
        }

        public void StopTick(StopReason reason, string user)
        {
            if (!IsRunning) return;
            StopReason = reason;
            StoppingUser = user;
            StopTick();
        }

        public void StopTick()
        {
            if (!IsRunning) return;
            ClockTimer.Stop();
            IsRunning = false;
            if (Options.Sounds.PlayAnnouncements) PlaySound(Options.Sounds.StopSoundFilePath);
        }

        #endregion Clock control

        private void Tick(object me, ElapsedEventArgs args)
        {
            IncreaseTime();
            if (PauseTime.HasValue && RealTime >= PauseTime.Value)
            {
                IsPaused = true;
                IsRealtime = ShowRealTimeWhenPaused;
                StopTick();
            }
            if (IsCompleted)
            {
                StopTick();
            }
        }

        private void IncreaseTime()
        {
            var previousElapsed = Elapsed;
            Elapsed = Elapsed.Add(TimeSpan.FromSeconds(Speed));
            if (Elapsed.Minutes != previousElapsed.Minutes)
                if (OnUpdate is not null) OnUpdate(this, Name);
        }
        public bool Update(string? userName, string? password, ClockSettings settings, IPAddress? ipAddress)
        {
            if (!IsAdministrator(password)) return false;

            UpdateUser(ipAddress, userName);
            RemoveInactiveUsers(TimeSpan.FromMinutes(30));
            var result = Update(settings);
            if (OnUpdate is not null) OnUpdate(this, Name);
            return result;
        }

        private bool Update(ClockSettings settings)
        {
            if (settings == null) return false;
            if (Name?.Equals(settings.Name, StringComparison.OrdinalIgnoreCase) != true) return false;
            if (!string.IsNullOrWhiteSpace(settings.AdministratorPassword)) AdministratorPassword = settings.AdministratorPassword;
            if (!string.IsNullOrWhiteSpace(settings.UserPassword)) UserPassword = settings.UserPassword;
            IsRealtime = settings.IsRealtime;
            StartDayAndTime = SetStartDayAndTime(settings.StartTime, settings.StartWeekday);
            Speed = settings.Speed ?? Speed;
            Duration = settings.Duration ?? Duration;
            PauseTime = settings.PauseTime;
            PauseReason = settings.PauseReason;
            ExpectedResumeTime = settings.ExpectedResumeTime;
            ShowRealTimeWhenPaused = settings.ShowRealTimeWhenPaused;
            Elapsed = settings.OverriddenElapsedTime.HasValue ? settings.OverriddenElapsedTime.Value - StartTime : Elapsed;
            Message = settings.Message ?? Message;
            if (settings.ShouldRestart) { Elapsed = TimeSpan.Zero; IsRunning = false; }
            if (settings.IsRunning) { TryStartTick(StoppingUser, AdministratorPassword); } else { StopTick(); }

            TimeSpan SetStartDayAndTime(TimeSpan? startTime, Weekday? startDay) =>
                new TimeSpan((int)(startDay ?? Weekday.NoDay), startTime?.Hours ?? Options.StartTime.Hours, startTime?.Minutes ?? Options.StartTime.Minutes, 0);
            return true;
        }


        public bool UpdateUser(IPAddress? ipAddress, string? userName, string? clientVersion = "")
        {
            const string Unknown = "Unknown";
            if (string.IsNullOrWhiteSpace(userName)) userName = Unknown;
            lock (Clients)
            {
                if (ipAddress is null) return false;
                if (HasSameUserNameWithOtherIpAddress(ipAddress, userName)) return false;
                var existing = Clients.Where(c => c.IPAddress.Equals(ipAddress)).ToArray();
                if (existing.Length == 0)
                {
                    Clients.Add(new ClockUser(ipAddress, userName, clientVersion));
                }
                else
                {
                    var unknown = existing.Where(e => Unknown.Equals(e.UserName, StringComparison.OrdinalIgnoreCase)).ToArray();
                    if (unknown.Length == 1) unknown[0].Update(userName, clientVersion);
                    else
                    {
                        var named = existing.Where(e => e.UserName?.Equals(userName, StringComparison.OrdinalIgnoreCase) == true).ToArray();
                        if (named.Length == 1) named[0].Update(userName, clientVersion);
                    }
                }
                return true;
            }
        }

        public void RemoveInactiveUsers(TimeSpan age)
        {
            lock (Clients)
            {
                foreach (var user in Clients.Where(c => c.LastUsedTime + age < DateTimeOffset.Now).ToList()) Clients.Remove(user);
            }
        }

        private bool HasSameUserNameWithOtherIpAddress(IPAddress? ipAddress, string? userName) =>
            Clients.Any(c => !c.IPAddress.Equals(ipAddress) && !string.IsNullOrWhiteSpace(c.UserName) && c.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

        private void ResetPause()
        {
            if (IsPaused)
            {
                IsPaused = false;
                PauseTime = null;
                ExpectedResumeTime = null;
                PauseReason = PauseReason.NoReason;
            }
        }

        private void ResetStopping()
        {
            StoppingUser = string.Empty;
            StopReason = StopReason.SelectStopReason;
        }

        public static void PlaySound(string? soundFilePath)
        {
            if (string.IsNullOrEmpty(soundFilePath)) return;
            if (File.Exists(soundFilePath)) Process.Start("powershell", $"-c (New-Object Media.SoundPlayer '{soundFilePath}').PlaySync();");
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork && a.ToString().StartsWith("192.", StringComparison.OrdinalIgnoreCase));
            return ip.Any() ? ip.First() : IPAddress.Loopback;
        }

        internal int SupportedVersion => (Options.Polling.IsEnabled || Options.Multicast.IsEnabled) ? 2 : 0;

        public string TcpMessage => string.Format(CultureInfo.InvariantCulture, "{1}{0}{2}{0}", NewLine, Level1Message, Level2Message);

        public string Level1Message => string.Format(CultureInfo.InvariantCulture, "{0} {1:hh} {1:mm} {2}", IsRunning ? 1 : 0, FastTime, Speed);

        public string Level2Message => string.Format(CultureInfo.InvariantCulture,
            @"clocktype={1}{0}clock={2:hh\:mm\:ss}{0}active={3}{0}weekday={4}{0}speed={5}{0}text={6}{0}interval={7}",
            Environment.NewLine,
            IsRealtime ? "realclock" : "fastclock",
            Time,
            IsRunning ? "yes" : "no",
            FastTime.WeekdayNumber(),
            Speed,
            Message.DefaultText ?? "",
            Options.Multicast?.IntervalSeconds
        );

        public string MulticastMessage => string.Format(CultureInfo.InvariantCulture,
            "fastclock{0}name={1}{0}version={2}{0}ip-address={3}{0}ip-port={4}{0}{5}",
            NewLine,
            Name,
            SupportedVersion,
            Options.Multicast.IsEnabled ? $"{GetLocalIPAddress()}" : "",
            Options.Multicast.IsEnabled ? $"{Options.Multicast.PortNumber}" : "",
            Level2Message);


        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ClockTimer?.Dispose();
                    Multicaster?.Dispose();
                    PollingService?.Dispose();
                }
                disposedValue = true;
            }
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
