using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using Tellurian.Trains.Clocks.Server;
using Tellurian.Trains.MeetingApp.Shared;

using ClockSettings = Tellurian.Trains.MeetingApp.Shared.ClockSettings;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS8604 // Possible null reference argument.

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
        [SwaggerResponse((int)HttpStatusCode.OK, "Available clocks were found.", typeof(IEnumerable<string>))]
        public IActionResult AvailableClocks() => Ok(Servers.Names);

        [SwaggerResponse((int)HttpStatusCode.OK, "Named clock was found.", typeof(ClockStatus))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpGet("[action]/{clock}")]
        public IActionResult Time([SwaggerParameter("Clock name", Required = true)] string clock) => Servers.Exists(clock) ? Ok(Servers.Instance(clock).GetStatus()) : (IActionResult)NotFound();

        [SwaggerResponse((int)HttpStatusCode.OK, "Settings for clock was found.", typeof(ClockSettings))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpGet("[action]/{clock}")]
        public IActionResult Settings([SwaggerParameter("Clock name", Required = true)] string clock) => Servers.Exists(clock) ? Ok(Servers.Instance(clock).GetSettings()) : (IActionResult)NotFound();

        [SwaggerResponse((int)HttpStatusCode.OK, "Clock was started")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpGet("[action]/{clock}")]
        public IActionResult Start(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromQuery, SwaggerParameter("User name")] string? user,
            [FromQuery, SwaggerParameter("Administrator password")] string? password)
        {
            if (!IsUser(apiKey, clock)) return Unauthorized();
            if (string.IsNullOrWhiteSpace(user)) return BadRequest($"{{ \"user\"={user} }}");
            if (Servers.Exists(clock))
            {
                return Servers.Instance(clock).StartTick(user, password) ? Ok() : (IActionResult)Unauthorized();
            }
            return NotFound();
        }

        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was stopped")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpGet("[action]/{clock}")]
        public IActionResult Stop(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromQuery, SwaggerParameter("User name", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Reason for stopping clock")] string? reason)
        {
            if (!IsUser(apiKey, clock)) return Unauthorized();
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

        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was updated")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [HttpPost("[action]/{clock}")]
        public IActionResult UpdateSettings(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromBody, SwaggerRequestBody("Clock settings", Required = true)] ClockSettings settings)
        {
            if (settings is null || string.IsNullOrWhiteSpace(clock)) return BadRequest();
            if (Servers.Exists(clock) && !IsAdministrator(apiKey, clock, settings.Password)) return Unauthorized();
            Servers.Instance(clock).Update(settings.AsSettings());
            return Ok();
        }

        private bool IsAdministrator(string? apiKey, string? clockName, string? password) =>
            !(clockName is null) && Servers.Exists(clockName) &&
            Servers.Instance(clockName).ApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase) &&
            Servers.Instance(clockName).Password.Equals(password, StringComparison.Ordinal);

        private bool IsUser(string? apiKey, string? clockName) =>
            !(clockName is null) && Servers.Exists(clockName) &&
            Servers.Instance(clockName).ApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase);
    }
}
