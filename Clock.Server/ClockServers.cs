using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tellurian.Trains.Clocks.Server
{
    public class ClockServers
    {
        private static readonly string Default = ClockSettings.DefaultName;
        public ClockServers(IOptions<ClockServerOptions> options)
        {
            Options = options;
            Servers = new Dictionary<string, IClockServer>
            {
                { Default.ToUpperInvariant(), new ClockServer(Options) {Name = Default, AdministratorPassword = ClockSettings.DefaultPassword } }
            };
        }
        private readonly IOptions<ClockServerOptions> Options;
        private readonly IDictionary<string, IClockServer> Servers;
        private DateTimeOffset LastRemovedInactiveClockServers { get; set; }

        public IClockServer Instance(string name)
        {
            lock (Servers)
            {
                RemoveInactiveClocks(TimeSpan.FromDays(2));
                if (string.IsNullOrWhiteSpace(name)) return Servers.Values.First();
                var key = name.ToUpperInvariant();
                if (!Servers.ContainsKey(key)) Servers.Add(key, new ClockServer(Options) { Name = name });
                return Servers[key];
            }
        }

        private void RemoveInactiveClocks(TimeSpan age)
        {
            var now = DateTimeOffset.Now;
            if (LastRemovedInactiveClockServers + TimeSpan.FromHours(1) < now)
            {
                foreach (var clockServer in (List<KeyValuePair<string, IClockServer>>)Servers.Where(s => s.Value.LastUsedTime + age < now).ToList()) Servers.Remove(clockServer.Key);
                LastRemovedInactiveClockServers = now;
            }
        }

        public IEnumerable<string> Names => Servers.Select(s => s.Value.Name);
        public bool Exists(string name) => !string.IsNullOrWhiteSpace(name) && Servers.ContainsKey(name.ToUpperInvariant());
    }
}
