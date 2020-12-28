using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using Tellurian.Trains.MeetingApp.Contract;
using Tellurian.Trains.MeetingApp.Contract.Extensions;

namespace Tellurian.Trains.MeetingApp.Controllers
{
    /// <summary>
    /// Controller for handling clock related actions.
    /// </summary>
    [Route("api/clocks")]
    public class ClockController : Controller
    {
        private const string ApiDocumentation = "https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/API-Guidelines";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="servers"></param>
        public ClockController(Clocks.Server.ClockServers servers)
        {
            Servers = servers;
        }

        private readonly Clocks.Server.ClockServers Servers;
        private IPAddress? RemoteIpAddress => Request.HttpContext.Connection.RemoteIpAddress;
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
        /// <param name="password">The clocks administratior password.</param>
        /// <returns>Array of strings with user name, IP-address and last time cloclk was accessed.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clock users was found.", typeof(IEnumerable<ClockUser>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Users")]
        public IActionResult Users(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password)

        {
            if (!Servers.Exists(clock)) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!IsAdministrator(clock, password)) return Unauthorized(AdministratorUnauthorizedErrorMessage());
            return Ok(Servers.Instance(clock).ClockUsers());
        }

        /// <summary>
        /// Gets current time and status for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get time and status from.</param>
        /// <param name="user">The name or station name of the user that makes the request.</param>
        /// <param name="clientVersion">The client version number. Optional.</param>
        /// <returns><see cref="ClockStatus"/></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Named clock was found.", typeof(ClockStatus))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Time")]
        public IActionResult Time(
            [SwaggerParameter("Clock name", Required = true)] string clock,
            [FromQuery, SwaggerParameter("Username")] string? user,
            [FromQuery, SwaggerParameter("Client version number")] string? clientVersion) =>
           Servers.Exists(clock) ?
            Ok(Servers.Instance(clock).AsApiContract(RemoteIpAddress, user, clientVersion)) :
            (IActionResult)NotFound(ClockNotFoundErrorMessage(clock));

        /// <summary>
        /// Gets current settings for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get settings from.</param>
        /// <param name="password">The administrator password is required to get the clock settings.</param>
        /// <returns><see cref="ClockSettings"/></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Settings for clock was found.", typeof(ClockSettings))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Settings")]
        public IActionResult Settings(
            [SwaggerParameter("Clock name", Required = true)] string clock,
            [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password)
        {
            if (!IsAdministrator(clock, password)) return Unauthorized(AdministratorUnauthorizedErrorMessage());
            return Servers.Exists(clock) ? Ok(Servers.Instance(clock).Settings.AsApiContract()) : (IActionResult)NotFound(ClockNotFoundErrorMessage(clock));
        }

        /// <summary>
        /// Starts or restarts game time. Only the user that stopped the clock or the administrator can restart the clock.
        /// </summary>
        /// <param name="clock">The clock name to start.</param>
        /// <param name="user">The name or station name of the user that tries to start the clock.</param>
        /// <param name="password">A clock user or clock administrator password.</param>
        /// <returns>Returns no data</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clock was started")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpPut("{clock}/start")]
        public IActionResult Start(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("Username", Required =true)] string? user,
            [FromQuery, SwaggerParameter("User or administrator password", Required = true)] string? password)
        {
            if (!IsUser(clock, password)) return Unauthorized(UserUnauthorizedErrorMessage());
            if (string.IsNullOrWhiteSpace(user)) return BadRequest(UserNameMissingErrorMessage(user));
            if (Servers.Exists(clock))
            {
                return Servers.Instance(clock).TryStartTick(user, password) ? Ok() : (IActionResult)Unauthorized(UserUnauthorizedErrorMessage());
            }
            return NotFound(ClockNotFoundErrorMessage(clock));
        }

        /// <summary>
        /// Starts or restarts game time. Only user with a name and given reason or the administrator can stop the clock.
        /// </summary>
        /// <param name="clock">The clock name to stop.</param>
        /// <param name="user">The name or station name of the user that want to stop the clock for a given reason.</param>
        /// <param name="reason">A predefined reason</param>
        /// <param name="password">A clock user or clock administrator password.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was stopped")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpPut("{clock}/stop")]
        public IActionResult Stop(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Reason for stopping clock", Required = true)] string? reason,
            [FromQuery, SwaggerParameter("User- or administrator password", Required = true)] string? password)
        {
            if (!Servers.Exists(clock)) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!IsUser(clock, password)) return Unauthorized(UserUnauthorizedErrorMessage());
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(reason))
            {
                return BadRequest(UserOrStopReasonMissingErrorMessage(user, reason));
            }
            else if (reason.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                Servers.Instance(clock).TryStopTick(user, password, Clocks.Contracts.StopReason.Other);
            }
            else
            {
                var stopReason = reason.AsStopReason();
                if (stopReason.IsInvalid()) return BadRequest(StopReasonInvalidErrorMessage(reason));
                Servers.Instance(clock).TryStopTick(user, password, reason.AsStopReason());
            }
            return Ok();
        }

        /// <summary>
        /// Updates the current user of the clock.
        /// </summary>
        /// <param name="clock">The clock name to update.</param>
        /// <param name="password">The user password to the clock.</param>
        /// <param name="user">The name or station name to set as user name.</param>
        /// <param name="client">Client version number.</param>
        /// <returns>Returns no data.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "User was added as a clock user.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock administrator password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "User name is already taken.")]
        [HttpPut("{clock}/user")]
        public IActionResult ClockUser(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("User password", Required = true)] string? password,
            [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Client version", Required = true)] string? client)
        {
            if (!Servers.Exists(clock)) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!IsUser(clock, password)) return Unauthorized(UserUnauthorizedErrorMessage());
            if (Servers.Instance(clock).UpdateUser(RemoteIpAddress, user, client)) return Ok();
            return Conflict(UserNameAlreadyTakenErrorMessage(user));
        }

        /// <summary>
        /// Updates the <see cref="ClockSettings"/> of a clock.
        /// </summary>
        /// <param name="clock">The clock name to update.</param>
        /// <param name="user">The name or station name of the user that want to update the clock settings.</param>
        /// <param name="password">Clock administrator password</param>
        /// <param name="settings"><see cref="ClockSettings"/></param>. Note that the password in the settings must match the clocks password.
        /// <returns>Returns no data.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was updated")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock administrator password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "User name and/or reason for stopping not provided.")]
        [HttpPut("{clock}/settings")]
        public IActionResult Update(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password,
            [FromBody, SwaggerRequestBody("Clock settings", Required = true)] ClockSettings settings)
        {
            if (settings is null || string.IsNullOrWhiteSpace(clock)) return BadRequest(SettingsErrorMessage(clock));
            if (Servers.Exists(clock) && !IsAdministrator(clock, password)) return Unauthorized(AdministratorUnauthorizedErrorMessage());
            if (Servers.Instance(clock).Update(user, password, settings.AsSettings(), RemoteIpAddress))
                return Ok();
            return Unauthorized(UserUnauthorizedErrorMessage());
        }

        private bool IsAdministrator(string? clockName, string? password) =>
            !string.IsNullOrWhiteSpace(clockName) && Servers.Instance(clockName).IsAdministrator(password);

        private bool IsUser(string? clockName, string? password) =>
            !string.IsNullOrWhiteSpace(clockName) && Servers.Exists(clockName) && Servers.Instance(clockName).IsUser(password);

        private static ErrorMessage ClockNotFoundErrorMessage(string? clockName) =>
            new ErrorMessage(HttpStatusCode.NotFound, ApiDocumentation, "ClockNotFound", new[] {
                $"Clock '{clockName}' does not exist. ",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "See documentation how to create a new clock."
            });
        private static ErrorMessage AdministratorUnauthorizedErrorMessage() =>
            new ErrorMessage(HttpStatusCode.Unauthorized, ApiDocumentation, "AdministratorUnauthorized", new[]
            {
                "The provided password is empty or is an accepted the administrator password."
            });

        private static ErrorMessage UserUnauthorizedErrorMessage() =>
            new ErrorMessage(HttpStatusCode.Unauthorized, ApiDocumentation, "UserUnauthorized", new[]
            {
                "The provided password is empty or is not a accepted user- or administrator password."
            });
        private static ErrorMessage StopReasonInvalidErrorMessage(string? reason) =>
            new ErrorMessage(HttpStatusCode.BadRequest, ApiDocumentation, "StopReasonInvalid", new[]
            {
                string.IsNullOrWhiteSpace(reason) ? "Stop reason is not provided." : $"Stop reason '{reason}' is invalid.",
                "Please, consult the documentation for the valid stop reasons."
            });

        private static ErrorMessage UserOrStopReasonMissingErrorMessage(string? user, string? reason) =>
            new ErrorMessage(HttpStatusCode.BadRequest, ApiDocumentation, "UserOrStopReasonMissing", new[]
            {
                string.IsNullOrWhiteSpace(user) ? "User name is not provided" : $"User name is '{user}.",
                string.IsNullOrWhiteSpace(reason) ? "Stop reason is not provided." : $"Stop reason is '{reason}.'"
            });

        private static ErrorMessage UserNameMissingErrorMessage(string? userName) =>
            new ErrorMessage(HttpStatusCode.BadRequest, ApiDocumentation, "UserNameMissing", new[]
            {
                string.IsNullOrWhiteSpace(userName) ? "User name is not provided." : $"User name '{userName}' is invalid.",
                "Please, consult the documentation for how to provide user name."
            });

        private static ErrorMessage SettingsErrorMessage(string? clockName) =>
             new ErrorMessage(HttpStatusCode.BadRequest, ApiDocumentation, "SettingsError", new[]
             {
                "API call payload does not contains valid settings or is empty.",
                string.IsNullOrWhiteSpace(clockName) ? "Clock name is not provided." : $"Clock name is '{clockName}'.",
                "Please, consult the documentation for how to provide correct settings."
             });

        private static ErrorMessage UserNameAlreadyTakenErrorMessage(string? user) =>
             new ErrorMessage(HttpStatusCode.Conflict, ApiDocumentation, "UserNameAlreadyTaken", new[]
             {
                string.IsNullOrWhiteSpace(user) ? "User name is not provided." : $"User name '{user}' is already taken.",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "Please, consult the documentation for how to create a clock with a new user name."
             });
    }
}
