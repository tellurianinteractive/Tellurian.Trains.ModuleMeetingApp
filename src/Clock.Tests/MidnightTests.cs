using Microsoft.Extensions.Logging.Abstractions;
using Tellurian.Trains.MeetingApp.Clocks.Implementations;
using Tellurian.Trains.MeetingApp.Contracts.Models;

namespace Tellurian.Trains.MeetingApp.Clocks.Tests;

[TestClass]
public class MidnightTests
{
    private static ClockServer CreateClock(TimeSpan startTime, Weekday weekday, TimeSpan? breakTime = null, TimeSpan? overriddenTime = null)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new ClockServerOptions
        {
            StartTime = startTime,
            Duration = TimeSpan.FromHours(12),
            Speed = 60
        });
        var target = new ClockServer(options, new TestTimeProvider { TestTime = DateTimeOffset.Parse("2022-12-01 13:00:00") }, new NullLogger<ClockServer>());
        target.UpdateSettings(IPAddress.Loopback, "Stefan", Settings.DefaultPassword, new Settings
        {
            StartTime = startTime,
            StartWeekday = weekday,
            Speed = 60,
            Duration = TimeSpan.FromHours(12),
            BreakTime = breakTime,
            OverriddenElapsedTime = overriddenTime,
        });
        return target;
    }

    private static void TickMinutes(ClockServer target, int minutes)
    {
        for (var i = 0; i < minutes; i++) target.Tick();
    }

    [TestMethod]
    public void WeekdayAdvancesAtMidnight()
    {
        var target = CreateClock(new TimeSpan(23, 50, 0), Weekday.Friday);
        TickMinutes(target, 9);
        Assert.AreEqual(Weekday.Friday, target.Status.Weekday);
        TickMinutes(target, 6);
        Assert.AreEqual(Weekday.Saturday, target.Status.Weekday);
        Assert.AreEqual("00:05", target.Status.AsApiContract().Time);
        Assert.AreEqual("Saturday", target.Status.AsApiContract().Weekday);
    }

    [TestMethod]
    public void SundayIsFollowedByMonday()
    {
        var target = CreateClock(new TimeSpan(23, 30, 0), Weekday.Sunday);
        TickMinutes(target, 45);
        Assert.AreEqual(Weekday.Monday, target.Status.Weekday);
        Assert.AreEqual("00:15", target.Status.AsApiContract().Time);
    }

    [TestMethod]
    public void NoWeekdayStaysWithoutWeekdayAfterMidnight()
    {
        var target = CreateClock(new TimeSpan(23, 30, 0), Weekday.NoDay);
        TickMinutes(target, 45);
        Assert.AreEqual(Weekday.NoDay, target.Status.Weekday);
        Assert.AreEqual(string.Empty, target.Status.AsApiContract().Weekday);
        Assert.AreEqual("00:15", target.Status.AsApiContract().Time);
    }

    [TestMethod]
    public void BreakAfterMidnightStopsClockAtBreakTime()
    {
        var target = CreateClock(new TimeSpan(23, 0, 0), Weekday.Monday, breakTime: new TimeSpan(0, 30, 0));
        TickMinutes(target, 89);
        Assert.IsFalse(target.IsBreak);
        target.Tick();
        Assert.IsTrue(target.IsBreak);
        Assert.AreEqual("00:30", target.Status.AsApiContract().Time);
        Assert.AreEqual(Weekday.Tuesday, target.Status.Weekday);
    }

    [TestMethod]
    public void BreakBeforeMidnightWithoutWeekdayStopsClockAtBreakTime()
    {
        var target = CreateClock(new TimeSpan(22, 0, 0), Weekday.NoDay, breakTime: new TimeSpan(23, 0, 0));
        TickMinutes(target, 59);
        Assert.IsFalse(target.IsBreak);
        target.Tick();
        Assert.IsTrue(target.IsBreak);
    }

    [TestMethod]
    public void OverriddenTimeAfterMidnightIsNextDay()
    {
        var target = CreateClock(new TimeSpan(22, 0, 0), Weekday.Wednesday, overriddenTime: new TimeSpan(1, 0, 0));
        Assert.AreEqual(TimeSpan.FromHours(3), target.Elapsed);
        Assert.AreEqual(Weekday.Thursday, target.Status.Weekday);
        Assert.AreEqual("01:00", target.Status.AsApiContract().Time);
    }

    [TestMethod]
    public void IsPassedBetweenHandlesMidnight()
    {
        var breakTime = new TimeSpan(0, 30, 0);
        Assert.IsTrue(breakTime.IsPassedBetween(new TimeSpan(1, 0, 29, 0), new TimeSpan(1, 0, 30, 0)));
        Assert.IsTrue(breakTime.IsPassedBetween(new TimeSpan(1, 23, 59, 0), new TimeSpan(2, 0, 31, 0)));
        Assert.IsFalse(breakTime.IsPassedBetween(new TimeSpan(1, 0, 30, 0), new TimeSpan(1, 0, 31, 0)));
        Assert.IsFalse(breakTime.IsPassedBetween(new TimeSpan(1, 23, 0, 0), new TimeSpan(1, 23, 59, 0)));
    }

    private static (ClockServer target, TestTimeProvider time) CreateClockAtRealTime(TimeSpan realTime, TimeSpan? pauseTime, TimeSpan? expectedResumeTime = null)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new ClockServerOptions
        {
            StartTime = TimeSpan.FromHours(8),
            Duration = TimeSpan.FromHours(12),
            Speed = 6
        });
        var time = new TestTimeProvider();
        var target = new ClockServer(options, time, new NullLogger<ClockServer>());
        time.TestTime = new DateTimeOffset(2022, 12, 1, 0, 0, 0, TimeSpan.Zero) + realTime - target.UtcOffset;
        target.UpdateSettings(IPAddress.Loopback, "Stefan", Settings.DefaultPassword, new Settings
        {
            StartWeekday = Weekday.Thursday,
            PauseTime = pauseTime,
            ExpectedResumeTime = expectedResumeTime,
        });
        return (target, time);
    }

    [TestMethod]
    public void PauseAfterMidnightDoesNotPauseBeforeMidnight()
    {
        var (target, time) = CreateClockAtRealTime(new TimeSpan(23, 0, 0), new TimeSpan(0, 30, 0));
        target.Tick();
        Assert.IsFalse(target.IsPaused);
        time.TestTime += TimeSpan.FromMinutes(89);
        target.Tick();
        Assert.IsFalse(target.IsPaused);
        time.TestTime += TimeSpan.FromMinutes(1);
        target.Tick();
        Assert.IsTrue(target.IsPaused);
    }

    [TestMethod]
    public void PauseTimeRecentlyPassedPausesAtOnce()
    {
        var (target, _) = CreateClockAtRealTime(new TimeSpan(14, 0, 0), new TimeSpan(13, 0, 0));
        target.Tick();
        Assert.IsTrue(target.IsPaused);
    }

    [TestMethod]
    public void PauseOverMidnightHasCorrectDuration()
    {
        var (target, _) = CreateClockAtRealTime(new TimeSpan(22, 0, 0), new TimeSpan(23, 50, 0), new TimeSpan(0, 10, 0));
        Assert.AreEqual(TimeSpan.FromMinutes(20), target.PauseDuration);
    }

    [TestMethod]
    public void PauseBeforeRealEndAfterMidnightIsIncludedInRealEndTime()
    {
        // 12 hours game at speed 6 gives 2 hours real time: ends 01:00, plus 20 minutes pause.
        var (target, _) = CreateClockAtRealTime(new TimeSpan(23, 0, 0), new TimeSpan(23, 30, 0), new TimeSpan(23, 50, 0));
        Assert.AreEqual("01:20", target.Status.AsApiContract().RealEndTime);
    }

    [TestMethod]
    public void IsElapsedIsFalseBeforeStartWhenWeekdayIsSet()
    {
        var target = CreateClock(new TimeSpan(23, 50, 0), Weekday.Friday);
        Assert.IsFalse(target.Settings.AsApiContract().IsElapsed);
        target.Tick();
        Assert.IsTrue(target.Settings.AsApiContract().IsElapsed);
    }

    [TestMethod]
    public void StatusIsElapsedOnlyAfterStart()
    {
        var target = CreateClock(new TimeSpan(8, 0, 0), Weekday.Monday);
        Assert.IsFalse(target.Status.AsApiContract().IsElapsed);
        target.Tick();
        target.StopTick();
        Assert.IsTrue(target.Status.AsApiContract().IsElapsed);
    }
}
