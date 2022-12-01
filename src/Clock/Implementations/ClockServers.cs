using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tellurian.Trains.MeetingApp.Clocks.Implementations
{
    public class ClockServers
    {
        private static readonly string Default = Settings.DefaultName;

        public ClockServers(IOptions<ClockServerOptions> options, ITimeProvider timeProvider, ILogger<ClockServer> logger)
        {
            Options = options;
            TimeProvider = timeProvider;
            Logger = logger;
            Servers = new Dictionary<string, IClock>
            {
                { Default.ToUpperInvariant(), new ClockServer(Options, TimeProvider, Logger) {Name = Default, AdministratorPassword = Settings.DefaultPassword } }
            };
        }
        private readonly IOptions<ClockServerOptions> Options;
        private readonly ITimeProvider TimeProvider;
        private readonly ILogger<ClockServer> Logger;
        private readonly IDictionary<string, IClock> Servers;
        private DateTimeOffset LastRemovedInactiveClockServers { get; set; }

        public IClock? Instance(string? name)
        {
            lock (Servers)
            {
                RemoveInactiveClocks(TimeSpan.FromDays(2));
                if (string.IsNullOrWhiteSpace(name)) return Servers.Values.First();
                var key = name.ToUpperInvariant();
                return Servers.ContainsKey(key) ? Servers[key] : null;
            }
        }

        public bool Create(string userName, Settings settings, IPAddress? remoteIpAddress)
        {
            if (string.IsNullOrWhiteSpace(settings.Name) || string.IsNullOrWhiteSpace(settings.AdministratorPassword)) return false;
            var key = settings.Name.ToUpperInvariant();
            lock (Servers)
            {
                if (Servers.ContainsKey(key)) return false;
                var clockServer = new ClockServer(Options, TimeProvider, Logger) { Name = settings.Name, AdministratorPassword = settings.AdministratorPassword };
                var created = clockServer.UpdateSettings(remoteIpAddress, userName, settings.AdministratorPassword, settings);
                if (created)
                {
                    Servers.Add(key, clockServer);
                    Logger.LogInformation("Clock '{name}' created.", clockServer.Name);
                }
                return created;
            }
        }

        private void RemoveInactiveClocks(TimeSpan age)
        {
            var now = DateTimeOffset.Now;
            if (LastRemovedInactiveClockServers + TimeSpan.FromHours(1) < now)
            {
                foreach (var clockServer in Servers.Where(s => s.Value.LastAccessedTime + age < now).ToList()) Servers.Remove(clockServer.Key);
                LastRemovedInactiveClockServers = now;
            }
        }

        public IEnumerable<string> Names => Servers.Select(s => s.Value.Name);
        public bool Exists(string? name) => !string.IsNullOrWhiteSpace(name) && Servers.ContainsKey(name.ToUpperInvariant());
    }
}
