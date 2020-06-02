using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable CA1034 // Nested types should not be visible: It is required for AppSettings

namespace Tellurian.Trains.Clocks.Server.Tests
{
    [TestClass]
    public class AppsettingsTests
    {
        [TestMethod]
        public void CreateAppsettingsExample()
        {
            var target = new AppSettings();
            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new TimeSpanConverter());
            File.WriteAllText("ExampleAppsettings.json",  JsonSerializer.Serialize(target, options ));
        }

        public class AppSettings
        {
            public ClockServerOptions ClockServerOptions { get; set; } = new ClockServerOptions
            {
                Name = "TellurianClock",
                Duration = TimeSpan.FromHours(15),
                Speed = 5.5,
                StartTime = TimeSpan.FromHours(6),
                Sounds = new SoundOptions
                {
                    PlayAnnouncements = true,
                     StartSoundFilePath= @"Sounds\Ringtone.wav",
                     StopSoundFilePath = @"Sounds\Ringtone.wav"
                },
                Multicast = new MulticastOptions
                {
                    IntervalSeconds = 2,
                    IPAddress = "239.50.50.20",
                    PortNumber = 2000,
                    IsEnabled = false
                },
                Polling = new PollingOptions
                {
                    IsEnabled = false,
                    PortNumber = 2500
                }
            };
        }

        public class TimeSpanConverter : JsonConverter<TimeSpan>
        {
            public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            {
                writer?.WriteStringValue(value.ToString(@"hh\:mm", CultureInfo.InvariantCulture));
            }
        }
    }
}
