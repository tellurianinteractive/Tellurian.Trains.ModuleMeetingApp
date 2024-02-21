using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tellurian.Trains.Clocks.Tests")]

namespace Tellurian.Trains.MeetingApp.Clocks;

public sealed class ClockServerOptions
{
    public static IOptions<ClockServerOptions> Default => Options.Create(new ClockServerOptions
    {
        StartTime = TimeSpan.FromHours(6),
        Duration = TimeSpan.FromHours(12),
        Speed = 6,
    });

    public string Name { get; set; } = "Demo";
    public string Password { get; set; } = "password";
    public TimeSpan StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public double Speed { get; set; }
    public string TimeZoneId { get; set; } = "Central Europe Standard Time";
}

