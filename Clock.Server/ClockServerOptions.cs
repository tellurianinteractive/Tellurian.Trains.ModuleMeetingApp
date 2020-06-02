using System.Runtime.CompilerServices;
using System;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("Tellurian.Trains.Clocks.Tests")]

namespace Tellurian.Trains.Clocks.Server
{
    public sealed class ClockServerOptions
    {
        public static IOptions<ClockServerOptions> Default => Options.Create( new ClockServerOptions
        {
            Name = "Kolding",
            StartTime = TimeSpan.FromHours(6),
            Duration = TimeSpan.FromHours(12),
            Speed = 6,
            Polling = new PollingOptions { IsEnabled=true, PortNumber= 2000},
            Multicast = new MulticastOptions
            {
                IsEnabled = true,
                IPAddress = "239.50.50.20",
                PortNumber = 2500,
                IntervalSeconds = 2
            }
        });

        public string Name { get; set; } = "TellurianClock";
        public string Password { get; set; } = "password";
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public double Speed { get; set; }
        public PollingOptions Polling { get; set; } = new PollingOptions();
        public MulticastOptions Multicast { get; set; } = new MulticastOptions();
        public SoundOptions Sounds { get; set; } = new SoundOptions();
        public string ApiKey { get; set; } = "tellurian";
    }

    public sealed class MulticastOptions
    {
        public bool IsEnabled { get; set; }
        public string? IPAddress { get; set; }
        public int PortNumber { get; set; }
        public int LocalPortNumber { get; set; }
        public int IntervalSeconds { get; set; }
    }

    public sealed class PollingOptions
    {
        public bool IsEnabled { get; set; }
        public int PortNumber { get; set; }
    }

    public sealed class SoundOptions
    {
        public bool PlayAnnouncements { get; set; } = false;
        public string? StartSoundFilePath { get; set; }
        public string? StopSoundFilePath { get; set; }
    }
}
