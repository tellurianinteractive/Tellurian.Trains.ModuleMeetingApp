using Microsoft.Extensions.Options;
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
            Servers = new Dictionary<string, ClockServer>
            {
                { Default.ToUpperInvariant(), new ClockServer(Options) }
            };
        }
        private readonly IOptions<ClockServerOptions> Options;
        private readonly IDictionary<string, ClockServer> Servers;

        public ClockServer Instance(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Servers.Values.First();
            var key = name.ToUpperInvariant();
            if (!Servers.ContainsKey(key)) Servers.Add(key, new ClockServer(Options) { Name = name }) ;
            return Servers[key];
        }

        public IEnumerable<string> Names => Servers.Select(s => s.Value.Name);
        public bool Exists(string name) => !string.IsNullOrWhiteSpace(name) && Servers.ContainsKey(name.ToUpperInvariant());
    }
}
