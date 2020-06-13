using Microsoft.AspNetCore.Mvc;
using System;
using Tellurian.Trains.Clocks.Server;
using Tellurian.Trains.MeetingApp.Shared;

using ClockSettings = Tellurian.Trains.MeetingApp.Shared.ClockSettings;

namespace Tellurian.Trains.MeetingApp.Controllers
{
    [Route("api/[controller]")]
    public class ClockController : Controller
    {
        public ClockController(ClockServers servers)
        {
            Servers = servers;
        }

        private readonly ClockServers Servers;

        [HttpGet("[action]")]
        public IActionResult AvailableClocks() => Ok(Servers.Names);

        [HttpGet("[action]/{clock}")]
        public IActionResult Time(string clock) => Servers.Exists(clock) ? Ok(Servers.Instance(clock).GetStatus()) : (IActionResult)NotFound();

        [HttpGet("[action]/{clock}")]
        public IActionResult Settings(string clock) => Servers.Exists(clock) ? Ok(Servers.Instance(clock).GetSettings()) : (IActionResult)NotFound();

        [HttpGet("[action]/{clock}")]
        public IActionResult Start(string clock, [FromQuery] string? apiKey, [FromQuery] string? user, [FromQuery] string? password)
        {
            if (IsUnauthorized(apiKey, clock)) return Unauthorized();
            if (Servers.Exists(clock))
            {
                return Servers.Instance(clock).StartTick(user, password) ? Ok() : (IActionResult)Unauthorized();
            }
            return NotFound();
        }

        [HttpGet("[action]/{clock}")]
        public IActionResult Stop(string clock, [FromQuery] string? apiKey, [FromQuery] string? user, [FromQuery] string? reason)
        {
            if (IsUnauthorized(apiKey, clock)) return Unauthorized();
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(reason))
            {
                return BadRequest($"{{ \"user\"={user}, \"reason\"={reason} }}");
            }
            else if (reason.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                if (Servers.Exists(clock))
                    Servers.Instance(clock).StopTick(StopReason.Other, user);
                else return NotFound();
            }
            else
            {
                var stopReason = reason.AsStopReason();
                if (stopReason == StopReason.SelectStopReason) return BadRequest("{ \"reason\": \"invalid\" }");
                if (Servers.Exists(clock))
                    Servers.Instance(clock).StopTick(reason.AsStopReason(), user);
                else return NotFound();
            }
            return Ok();
        }

        [HttpPost("[action]/{clock}")]
        public IActionResult UpdateSettings(string clock, [FromQuery] string? apiKey, [FromBody] ClockSettings settings)
        {
            if (IsUnauthorized(apiKey, clock)) return Unauthorized();
            Servers.Instance(clock).Update(settings.AsSettings());
            return Ok();
        }

        private bool IsUnauthorized(string? apiKey, string clock) =>
            !(ClockSettings.ClockApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase) || Servers.Instance(clock).ApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase));
    }
}
