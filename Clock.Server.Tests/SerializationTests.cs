using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace Tellurian.Trains.Clocks.Server.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void ClockStatusSerilizeAndDeSerialize()
        {
            var target = new MeetingApp.Shared.ClockSettings()
            {
                DurationHours = 15.5,
                ExpectedResumeTime = string.Empty,
                IsElapsed = true,
                IsRunning = true,
                Message = string.Empty,
                Mode = "1",
                Name = "Test",
                OverriddenElapsedTime = string.Empty,
                Password = "password",
                PauseReason = "NoReason",
                PauseTime = string.Empty,
                ShouldRestart = false,
                ShowRealTimeWhenPaused = true,
                Speed = 5.5,
                StartTime = "06:00",
                StartWeekday = "Monday"
            };
            var json = JsonSerializer.Serialize(target);
            var actual = JsonSerializer.Deserialize<MeetingApp.Shared.ClockSettings>(json);
            Assert.IsNotNull(actual);
            Assert.AreEqual(target.DurationHours, actual.DurationHours);
        }
    }
}
