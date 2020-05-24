using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Tellurian.Trains.Clocks.Server;
using Tellurian.Trains.MeetingApp.Shared;

using ClockSettings = Tellurian.Trains.MeetingApp.Shared.ClockSettings;

namespace Tellurian.Trains.MeetingApp.Controllers
{
    [Route("api/[controller]")]
    public class ClockController : Controller
    {
        public ClockController(ClockServer server)
        {
            Server = server;
        }

        private readonly ClockServer Server;

        [HttpGet("[action]")]
        public IEnumerable<string> AvailableClocks()
        {
            return new[] { "Geflemodul", "Kolding" };
        }

        [HttpGet("[action]")]
        public ClockStatus Time()
        {
            return Server.GetStatus();
        }

        [HttpGet("[action]")]
        public ClockSettings Settings()
        {
            return Server.GetSettings();
        }

        [HttpGet("[action]")]
        public void Start()
        {
            Server.StartTick();
        }

        [HttpGet("[action]")]
        public void Stop([FromQuery] string? user, [FromQuery] string? reason)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(reason)) return;
            Server.StopTick(reason.AsStopReason(), user);
        }

        [HttpPost("[action]")]
        public void UpdateSettings([FromBody] ClockSettings settings)
        {
            Server.Update(settings.AsSettings());
        }
    }
}
