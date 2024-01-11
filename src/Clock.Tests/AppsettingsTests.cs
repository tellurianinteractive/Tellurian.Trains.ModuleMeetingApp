using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tellurian.Trains.MeetingApp.Clocks.Tests;

[TestClass]
public class AppsettingsTests
{
    readonly JsonSerializerOptions options = new() { WriteIndented = true };

    [TestMethod]
    public void CreateAppsettingsExample()
    {
        var target = new AppSettings();
        options.Converters.Add(new TimeSpanConverter());
        File.WriteAllText("ExampleAppsettings.json", JsonSerializer.Serialize(target, options));
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
                StartSoundFilePath = @"Sounds\Ringtone.wav",
                StopSoundFilePath = @"Sounds\Ringtone.wav"
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
