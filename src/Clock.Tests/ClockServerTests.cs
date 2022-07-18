using Microsoft.Extensions.Options;
using Tellurian.Trains.MeetingApp.Clocks.Implementations;
using Tellurian.Trains.MeetingApp.Contracts;

namespace Tellurian.Trains.MeetingApp.Clocks.Tests;

[TestClass]
public class ClockServerTests
{
    readonly IOptions<ClockServerOptions> Options = Microsoft.Extensions.Options.Options.Create(new ClockServerOptions());

    [TestMethod] public void AddsNewUser()
    {
        var target = new ClockServer(Options);
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), "Stefan",  "1.2.3");
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.AreEqual("Stefan", user.UserName);
        Assert.AreEqual("192.168.1.2", user.IPAddress.ToString());
        Assert.AreEqual("1.2.3", user.ClientVersion);
        Assert.IsTrue((DateTime.Now - user.LastUsedTime) < TimeSpan.FromMilliseconds(2));
    }


    [TestMethod]
    public async Task UpdatesUser()
    {
        var target = new ClockServer(Options);
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
        var target = new ClockServer(Options);
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), ClockSettings.UnknownUserName, "1.2.3");
        await Task.Delay(1);
        target.UpdateUser(IPAddress.Parse("192.168.1.2"), "Stefan", "1.2.3");
        Assert.AreEqual(1, target.ClockUsers.Count());
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.IsTrue((DateTime.Now - user.LastUsedTime) < TimeSpan.FromMilliseconds(2));
    }
}
