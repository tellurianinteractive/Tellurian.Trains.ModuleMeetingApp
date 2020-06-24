using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using Tellurian.Trains.MeetingApp.Shared;

#pragma warning disable CS8604 // Possible null reference argument.

namespace Tellurian.Trains.MeetingApp.Controllers
{
    /// <summary>
    /// Controller for handling clock related actions.
    /// </summary>
    [Route("api/clocks")]
    public class ClockController : Controller
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="servers"></param>
        public ClockController(Clocks.Server.ClockServers servers)
        {
            Servers = servers;
        }

        private readonly Clocks.Server.ClockServers Servers;
        private IPAddress RemoteIpAddress => Request.HttpContext.Connection.RemoteIpAddress;
        /// <summary>
        /// Gets a list with currently available clocks.
        /// </summary>
        /// <returns>Array och clock names.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Available clocks were found.", typeof(IEnumerable<string>))]
        [Produces("application/json", "text/json")]
        [HttpGet("available")]
        public IActionResult Available() => Ok(Servers.Names);

        /// <summary>
        /// Gets current clock users for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get users from.</param>
        /// <param name="apiKey">The clocks API-key.</param>
        /// <param name="password">The clocks administratior password.</param>
        /// <returns>Array of strings with user name, IP-address and last time cloclk was accessed.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clock users was found.", typeof(IEnumerable<ClockUser>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Users")]
        public IActionResult Users(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password)

        {
            if (!Servers.Exists(clock)) return NotFound();
            if (!IsAdministrator(apiKey, clock, password)) return Unauthorized();
            return Ok(Servers.Instance(clock).ClockUsers());
        }

        /// <summary>
        /// Gets current time and status for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get time and status from.</param>
        /// <param name="user">The name or station name of the user that makes the request.</param>
        /// <returns><see cref="ClockStatus"/></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Named clock was found.", typeof(ClockStatus))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Time")]
        public IActionResult Time(
            [SwaggerParameter("Clock name", Required = true)] string clock,
            [FromQuery, SwaggerParameter("User name")] string? user) =>
            Servers.Exists(clock) ? Ok(Servers.Instance(clock).GetStatus(RemoteIpAddress, user)) : (IActionResult)NotFound();

        /// <summary>
        /// Gets current settings for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get settings from.</param>
        /// <returns><see cref="ClockSettings"/></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Settings for clock was found.", typeof(ClockSettings))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Settings")]
        public IActionResult Settings(
            [SwaggerParameter("Clock name", Required = true)] string clock) =>
            Servers.Exists(clock) ? Ok(Servers.Instance(clock).GetSettings()) : (IActionResult)NotFound();

        /// <summary>
        /// Starts or restarts game time. Only the user that stopped the clock or the administrator can restart the clock.
        /// </summary>
        /// <param name="clock">The clock name to start.</param>
        /// <param name="apiKey">The clocks API-key.</param>
        /// <param name="user">The name or station name of the user that tries to start the clock.</param>
        /// <param name="password">A clock user or clock administrator password.</param>
        /// <returns>Returns no data</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clock was started")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpPut("{clock}/start")]
        public IActionResult Start(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromQuery, SwaggerParameter("User name")] string? user,
            [FromQuery, SwaggerParameter("User or administrator password")] string? password)
        {
            if (!IsUser(apiKey, clock, password)) return Unauthorized();
            if (string.IsNullOrWhiteSpace(user)) return BadRequest($"{{ \"user\"={user} }}");
            if (Servers.Exists(clock))
            {
                return Servers.Instance(clock).StartTick(user, password) ? Ok() : (IActionResult)Unauthorized();
            }
            return NotFound();
        }

        /// <summary>
        /// Starts or restarts game time. Only user with a name and given reason or the administrator can stop the clock.
        /// </summary>
        /// <param name="clock">The clock name to stop.</param>
        /// <param name="apiKey">The clocks API-key.</param>
        /// <param name="user">The name or station name of the user that want to stop the clock for a given reason.</param>
        /// <param name="reason">A predefined reason in <see cref="Clocks.Server.StopReason"/></param>
        /// <param name="password">A clock user or clock administrator password.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was stopped")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpPut("{clock}/stop")]
        public IActionResult Stop(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromQuery, SwaggerParameter("User name", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Reason for stopping clock")] string? reason,
            [FromQuery, SwaggerParameter("User or administrator password")] string? password)
        {
            if (!Servers.Exists(clock)) return NotFound();
            if (!IsUser(apiKey, clock, password)) return Unauthorized();
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(reason))
            {
                return BadRequest($"{{ \"user\"={user}, \"reason\"={reason} }}");
            }
            else if (reason.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                Servers.Instance(clock).StopTick(Clocks.Server.StopReason.Other, user);
            }
            else
            {
                var stopReason = reason.AsStopReason();
                if (stopReason == Clocks.Server.StopReason.SelectStopReason) return BadRequest("{ \"reason\": \"invalid\" }");
                Servers.Instance(clock).StopTick(reason.AsStopReason(), user);
            }
            return Ok();
        }

        /// <summary>
        /// Updates the current user of the clock.
        /// </summary>
        /// <param name="clock">The clock name to update.</param>
        /// <param name="apiKey">The clocks API-key.</param>
        /// <param name="user">The name or station name to set as user name.</param>
        /// <param name="client">Client version number.</param>
        /// <returns>Returns no data.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was stopped")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "User name is already occupied.")]
        [HttpPut("{clock}/user")]
        public IActionResult ClockUser(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromQuery, SwaggerParameter("User name", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Client version", Required = true)] string? client)
        {
            if (!Servers.Exists(clock)) return NotFound();
            if (!IsValidApiKey(apiKey, clock)) return Unauthorized();
            if (Servers.Instance(clock).UpdateUser(RemoteIpAddress, user, client)) return Ok();
            return Conflict($"{{ \"Error\" = \"User name '{user}' is already occupied\" }}");
        }

        /// <summary>
        /// Updates the <see cref="ClockSettings"/> of a clock.
        /// </summary>
        /// <param name="clock">The clock name to update.</param>
        /// <param name="apiKey">The clocks API-key.</param>
        /// <param name="user">The name or station name of the user that want to update the clock settings.</param>
        /// <param name="password">Clock administrator password</param>
        /// <param name="settings"><see cref="ClockSettings"/></param>. Note that the password in the settings must match the clocks password.
        /// <returns>Returns no data.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was updated")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, API-key and/or clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [HttpPost("{clock}/Update")]
        public IActionResult Update(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("API-key", Required = true)] string? apiKey,
            [FromQuery, SwaggerParameter("User Name", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password,
            [FromBody, SwaggerRequestBody("Clock settings", Required = true)] ClockSettings settings)
        {
            if (settings is null || string.IsNullOrWhiteSpace(clock)) return BadRequest();
            if (Servers.Exists(clock) && !IsAdministrator(apiKey, clock, password)) return Unauthorized();
            Servers.Instance(clock).Update(settings.AsSettings(), RemoteIpAddress, user);
            return Ok();
        }

        private bool IsAdministrator(string? apiKey, string? clockName, string? password) =>
            IsValidApiKey(apiKey, clockName) &&
            Servers.Instance(clockName).IsAdministrator(password);

        private bool IsUser(string? apiKey, string? clockName, string? password) =>
            IsValidApiKey(apiKey, clockName) &&
            Servers.Instance(clockName).IsUser(password);

        private bool IsValidApiKey(string? apiKey, string? clockName) =>
            !(clockName is null) && Servers.Exists(clockName) &&
            Servers.Instance(clockName).ApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase);
    }
}
