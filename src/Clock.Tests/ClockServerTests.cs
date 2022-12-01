using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Tellurian.Trains.MeetingApp.Clocks.Implementations;
using Tellurian.Trains.MeetingApp.Contracts;

namespace Tellurian.Trains.MeetingApp.Clocks.Tests;

[TestClass]
public class ClockServerTests
{
    readonly IOptions<ClockServerOptions> Options = Microsoft.Extensions.Options.Options.Create(new ClockServerOptions());

    [TestMethod]
    public void AddsNewUser()
    {
        var target = new ClockServer(Options, new TestTimeProvider(), new NullLogger<ClockServer>());
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), "Stefan", "1.2.3");
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.AreEqual("Stefan", user.UserName);
        Assert.AreEqual("192.168.1.2", user.IPAddress.ToString());
        Assert.AreEqual("1.2.3", user.ClientVersion);
        Assert.IsTrue((DateTime.Now - user.LastUsedTime) < TimeSpan.FromMilliseconds(200));
    }


    [TestMethod]
    public async Task UpdatesUser()
    {
        var target = new ClockServer(Options, new TestTimeProvider(), new NullLogger<ClockServer>());
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), "Stefan", "1.2.3");
        await Task.Delay(1);
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), "Stefan", "1.2.3");
        Assert.AreEqual(1, target.ClockUsers.Count());
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.IsTrue((DateTime.Now - user.LastUsedTime) < TimeSpan.FromMilliseconds(2));
    }

    [TestMethod]
    public async Task UpdatesUnknownUser()
    {
        var target = new ClockServer(Options, new TestTimeProvider(), new NullLogger<ClockServer>());
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), ClockSettings.UnknownUserName, "1.2.3");
        await Task.Delay(1);
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), "Stefan", "1.2.3");
        Assert.AreEqual(1, target.ClockUsers.Count());
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.IsTrue((DateTime.Now - user.LastUsedTime) < TimeSpan.FromMilliseconds(2));
    }

    [TestMethod]
    public void GameRealEndTimeWithoutPause()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new ClockServerOptions
        {
            StartTime = TimeSpan.FromHours(8),
            Duration = TimeSpan.FromHours(10),
            Speed = 6
        }); ;
        var target = new ClockServer(options, new TestTimeProvider { TestTime = DateTimeOffset.Parse("2022-12-01 13:00:00") }, new NullLogger<ClockServer>());
        Assert.AreEqual(new TimeSpan(4, 15, 40, 0), target.RealEndAndDayTimeWithPause);     
    }

    [TestMethod]
    public void GameRealEndTimeWithPause()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new ClockServerOptions
        {
            StartTime = TimeSpan.FromHours(8),
            Duration = TimeSpan.FromHours(10),
            Speed = 6
        }); ;
        var target = new ClockServer(options, new TestTimeProvider { TestTime = DateTimeOffset.Parse("2022-12-01 13:00:00") }, new NullLogger<ClockServer>());
        var settings = target.Settings.AsApiContract();
        settings.PauseTime = "13:30";
        settings.ExpectedResumeTime = "13:45";
        target.UpdateSettings(IPAddress.Loopback, "Stefan", ClockSettings.DemoClockPassword, settings.AsSettings());
        Assert.AreEqual(TimeSpan.FromMinutes(15), target.PauseDuration);
        Assert.AreEqual(new TimeSpan(4, 15, 55, 0), target.RealEndAndDayTimeWithPause);
    }

    [TestMethod]
    public void GameRealEndTimeWithPauseStartingAfterGameEnd()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new ClockServerOptions
        {
            StartTime = TimeSpan.FromHours(8),
            Duration = TimeSpan.FromHours(10),
            Speed = 6
        }); ;
        var target = new ClockServer(options, new TestTimeProvider { TestTime = DateTimeOffset.Parse("2022-12-01 13:00:00") }, new NullLogger<ClockServer>());
        var settings = target.Settings.AsApiContract();
        settings.PauseTime = "16:00";
        settings.ExpectedResumeTime = "16:15";
        target.UpdateSettings(IPAddress.Loopback, "Stefan", ClockSettings.DemoClockPassword, settings.AsSettings());
        Assert.AreEqual(TimeSpan.FromMinutes(15), target.PauseDuration );
        Assert.AreEqual( new TimeSpan(4, 15, 40, 0), target.RealEndAndDayTimeWithPause);
    }
}

public class TestTimeProvider : ITimeProvider
{
    public DateTimeOffset TestTime { get; set; }
    public DateTimeOffset UtcNow => TestTime;
}
