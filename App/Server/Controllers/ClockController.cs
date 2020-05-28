using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet("[action]/{clock}")]
        public IActionResult Time(string clock)
        {
            if ("Default".Equals(clock, StringComparison.OrdinalIgnoreCase))
            {
                return Ok(Server.GetStatus());
            }
            return NotFound();
        }

        [HttpGet("[action]/{clock}")]
        public IActionResult Settings(string clock)
        {
            if ("Default".Equals(clock, StringComparison.OrdinalIgnoreCase))
            {
                return Ok(Server.GetSettings());
            }
            return NotFound();
        }

        [HttpGet("[action]/{clock}")]
        public IActionResult Start(string clock, [FromQuery] string? apiKey)
        {
            if (IsUnauthorized(apiKey)) return Unauthorized();
            if ("Default".Equals(clock, StringComparison.OrdinalIgnoreCase))
            {
                Server.StartTick();
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("[action]/{clock}")]
        public IActionResult Stop(string clock, [FromQuery] string? apiKey, [FromQuery] string? user, [FromQuery] string? reason)
        {
            if (IsUnauthorized(apiKey)) return Unauthorized();
            if ("Default".Equals(clock, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(reason))
                {
                    return BadRequest($"{{ \"user\"={user}, \"reason\"={reason} }}");
                }
                else
                {
                    var stopReason = reason.AsStopReason();
                    if (stopReason == StopReason.SelectStopReason) return BadRequest("{ \"reason\": \"invalid\" }");
                    Server.StopTick(reason.AsStopReason(), user);
                    return Ok();
               }
            }
            return NotFound();
        }

        [HttpPost("[action]/{clock}")]
        public IActionResult UpdateSettings(string clock, [FromQuery] string? apiKey, [FromBody] ClockSettings settings)
        {
            if (IsUnauthorized(apiKey)) return Unauthorized();
            if ("Default".Equals(clock, StringComparison.OrdinalIgnoreCase))
            {
                Server.Update(settings.AsSettings());
                return Ok();
            }
            return NotFound();
        }

        private bool IsUnauthorized(string? apiKey) =>
            !(ClockSettings.ClockApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase) || Server.ApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase));
    }
}
