//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Tellurian.Trains.MeetingApp.Clocks;
using Tellurian.Trains.MeetingApp.Clocks.Implementations;
using Tellurian.Trains.MeetingApp.Contracts;
using Tellurian.Trains.MeetingApp.Server.Extensions;

namespace Tellurian.Trains.MeetingApp.Server.Controllers
{
    /// <summary>
    /// Controller for handling clock related actions.
    /// </summary>
    /// <remarks>
    /// Constructor.
    /// </remarks>
    /// <param name="servers"></param>
    [ApiController]
    [Route("api/clocks")]
    public class ClockController(ClockServers servers) : Controller
    {
        private const string ApiDocumentation = "https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/API-Guidelines";
        private readonly ClockServers Servers = servers;

        private IPAddress? RemoteIpAddress => Request.HttpContext.Connection.RemoteIpAddress;
        [HttpGet("version")]
        public IActionResult GetVersion() => Ok(AppVersion.ServerVersion!.ToString(2));
        /// <summary>
        /// Gets a list with currently available clocks.
        /// </summary>
        /// <returns>Array och clock names.</returns>
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK, "application/json", "text/json")]
        [HttpGet("available")]
        public IActionResult Available() => Ok(Servers.Names);

        /// <summary>
        /// Gets current clock users for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get users from.</param>
        /// <param name="password">The clocks user- or administratior password.</param>
        /// <returns>Array of strings with user name, IP-address and last time cloclk was accessed.</returns>
        [ProducesResponseType(typeof(IEnumerable<ClockUser>), StatusCodes.Status200OK, "application/json", "text/json",
            Description = "Clock users was found.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized, "application/json", "text/json",
            Description = "Not authorized, clock password is not correct.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [HttpGet("{clock}/Users")]
        public IActionResult Users([Required] string? clock, [FromQuery] string? password)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            var instance = Servers.Instance(clock);
            if (instance == null) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!instance.IsUser(password)) return Unauthorized(UserUnauthorizedErrorMessage(clock));
            return Ok(instance.ClockUsers
                .Select(u => new ClockUser
                {
                    ClientVersion = u.ClientVersion,
                    IPAddress = u.IPAddress.ToString(),
                    LastUsedTime = u.LastUsedTime.ToString("T"),
                    UserName = u.UserName
                }));
        }

        /// <summary>
        /// Gets current time and status for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get time and status from.</param>
        /// <param name="user">The name or station name of the user that makes the request.</param>
        /// <param name="clientVersion">The client version number. Optional.</param>
        /// <returns><see cref="ClockStatus"/></returns>
        [ProducesResponseType(typeof(ClockStatus), StatusCodes.Status200OK, "application/json", "text/json",
            Description = "Named clock was found.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [HttpGet("{clock}/Time")]
        public IActionResult Time([Required] string clock, [FromQuery] string? user, [FromQuery] string? clientVersion)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            var instance = Servers.Instance(clock);
            if (instance == null) return NotFound(ClockNotFoundErrorMessage(clock));
            var status = instance.AsApiContract(RemoteIpAddress, user, clientVersion);
            status.ServerVersionNumber = AppVersion.ServerVersion?.ToString() ?? "";
            return Ok(status);
        }

        /// <summary>
        /// Gets current settings for a clock.
        /// </summary>
        /// <param name="clock">The clock name to get settings from.</param>
        /// <param name="password">The administrator password is required to get the clock settings.</param>
        /// <returns><see cref="ClockSettings"/></returns>
        [ProducesResponseType(typeof(ClockSettings), StatusCodes.Status200OK, "application/json", "text/json",
            Description = "Settings for clock was found.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized, "application/json", "text/json",
            Description = "Not authorized, clock password is not correct.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [HttpGet("{clock}/settings")]
        public IActionResult Settings([Required] string clock, [FromQuery] string? password)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            var instance = Servers.Instance(clock);
            if (instance == null) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!instance.IsAdministrator(password)) return Unauthorized(AdministratorUnauthorizedErrorMessage(clock));
            return Ok(instance.Settings.AsApiContract());
        }

        /// <summary>
        /// Starts or restarts game time. Only the user that stopped the clock or the administrator can restart the clock.
        /// </summary>
        /// <param name="clock">The clock name to start.</param>
        /// <param name="user">The name or station name of the user that tries to start the clock.</param>
        /// <param name="password">A clock user or clock administrator password.</param>
        /// <returns>Returns no data</returns>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK, "application/json", "text/json",
            Description = "Clock was started.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized, "application/json", "text/json",
            Description = "Not authorized, clock password is not correct.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [HttpPut("{clock}/start")]
        public IActionResult Start([Required] string? clock, [FromQuery] string? user, [FromQuery] string? password)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            if (string.IsNullOrWhiteSpace(user)) return BadRequest(UserNameMissingErrorMessage(clock));
            var instance = Servers.Instance(clock);
            if (instance is null) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!instance.IsUser(password)) return Unauthorized(UserUnauthorizedErrorMessage(clock));
            return instance.TryStartTick(user, password) ? Ok() : Unauthorized(UserUnauthorizedErrorMessage(clock));
        }

        /// <summary>
        /// Starts or restarts game time. Only user with a name and given reason or the administrator can stop the clock.
        /// </summary>
        /// <param name="clock">The clock name to stop.</param>
        /// <param name="user">The name or station name of the user that want to stop the clock for a given reason.</param>
        /// <param name="reason">A predefined reason</param>
        /// <param name="password">A clock user or clock administrator password.</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK, "application/json", "text/json",
            Description = "Clock was stopped.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized, "application/json", "text/json",
            Description = "Not authorized, clock password is not correct.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [HttpPut("{clock}/stop")]
        public IActionResult Stop([Required] string? clock, [FromQuery] string? user, [FromQuery] string? reason, [FromQuery] string? password)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            if (string.IsNullOrWhiteSpace(user)) return BadRequest(UserNameMissingErrorMessage(user));
            var instance = Servers.Instance(clock);
            if (instance is null) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!instance.IsUser(password)) return Unauthorized(UserUnauthorizedErrorMessage(clock));
            var stopReason = reason.AsStopReason();
            if (stopReason.IsInvalid()) return BadRequest(StopReasonInvalidErrorMessage(reason));
            return instance.TryStopTick(user, password, reason.AsStopReason()) ? Ok() : Unauthorized(UserUnauthorizedErrorMessage(clock));
        }

        /// <summary>
        /// Updates the current user of the clock.
        /// </summary>
        /// <param name="clock">The clock name to update.</param>
        /// <param name="password">The user password to the clock.</param>
        /// <param name="user">The name or station name to set as user name.</param>
        /// <param name="client">Client version number.</param>
        /// <returns>Returns no data.</returns>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK, "application/json", "text/json",
            Description = "User was added as a clock user.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized, "application/json", "text/json",
            Description = "Not authorized, clock administrator password is not correct.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status409Conflict, "application/json", "text/json",
            Description = "User name is already taken.")]
        [HttpPut("{clock}/user")]
        public IActionResult ClockUser([Required] string? clock, [FromQuery] string? password, [FromQuery] string? user, [FromQuery] string? client)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            if (string.IsNullOrWhiteSpace(user)) return BadRequest(UserNameMissingErrorMessage(clock));
            var instance = Servers.Instance(clock);
            if (instance == null) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!instance.IsUser(password)) return Unauthorized(UserUnauthorizedErrorMessage(clock));
            return instance.UpdateUser(RemoteIpAddress, user, client) ? Ok() : Conflict(UserNameAlreadyTakenErrorMessage(user));
        }

        /// <summary>
        /// Create a new clock the <see cref="ClockSettings"/> of a clock.
        /// </summary>
        /// <param name="user">The name or station name of the user that want to update the clock settings.</param>
        /// <param name="settings"><see cref="ClockSettings"/></param>. 
        /// <returns>Returns no data.</returns>
        [ProducesResponseType(typeof(ClockSettings), StatusCodes.Status201Created, "application/json", "text/json",
            Description = "Clock was created.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status409Conflict, "application/json", "text/json",
            Description = "Clock name is already taken.")]
        [HttpPost("create")]
        public IActionResult Create([FromQuery] string? user, [FromBody] ClockSettings settings)
        {
            if (string.IsNullOrWhiteSpace(user)) return BadRequest(UserNameMissingErrorMessage(user));
            if (settings is null || string.IsNullOrWhiteSpace(settings.Name) || string.IsNullOrWhiteSpace(settings.AdministratorPassword)) return BadRequest(SettingsErrorMessage(nameof(settings)));
            var clock = settings.Name;
            var instance = Servers.Instance(clock);
            if (instance is not null) return Conflict(new ErrorMessage(HttpStatusCode.Conflict, ApiDocumentation, "ClockAlreadyExists", [$"Clock name '{clock}' is already taken."]));
            if (Servers.Create(user, settings.AsSettings(), RemoteIpAddress))
            {
                instance = Servers.Instance(clock);
                if (instance is not null) return Create($"{Request.Host}/api/{clock}/settings", instance.Settings.AsApiContract());
            }
            return BadRequest(SettingsErrorMessage(clock));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK, "application/json", "text/json",
            Description = "Clock was updated.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest, "application/json", "text/json",
            Description = "Request has incomplete data, see error response for details.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized, "application/json", "text/json",
            Description = "Not authorized, clock administrator password is not correct.")]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status404NotFound, "application/json", "text/json",
            Description = "Named clock does not exist.")]
        [HttpPut("{clock}/settings")]
        public IActionResult Update([Required] string? clock, [FromQuery] string? user, [FromQuery] string? password, [FromBody] ClockSettings settings)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            if (settings is null) return BadRequest(SettingsErrorMessage(clock));
            var instance = Servers.Instance(clock);
            if (instance is null) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!instance.IsAdministrator(password)) return Unauthorized(UserUnauthorizedErrorMessage(clock));
            return instance.UpdateSettings(RemoteIpAddress, user, password, settings.AsSettings()) ? Ok() : Unauthorized(UserUnauthorizedErrorMessage(clock));
        }

        private static ErrorMessage ClockNameMissingErrorMessage() =>
           new(HttpStatusCode.NotFound, ApiDocumentation, "ClockNotFound", [
                $"Clock name was not provided.",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "See documentation how to create a new clock."
           ]);
        private static ErrorMessage ClockNotFoundErrorMessage(string? clockName) =>
            new(HttpStatusCode.NotFound, ApiDocumentation, "ClockNotFound", [
                $"Clock '{clockName}' does not exist. ",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "See documentation how to create a new clock."
            ]);
        private static ErrorMessage AdministratorUnauthorizedErrorMessage(string clockName) =>
            new(HttpStatusCode.Unauthorized, ApiDocumentation, "AdministratorUnauthorized",
            [
                $"The provided password for clock '{clockName}' is empty or is an accepted the administrator password."
            ]);

        private static ErrorMessage UserUnauthorizedErrorMessage(string clockName) =>
            new(HttpStatusCode.Unauthorized, ApiDocumentation, "UserUnauthorized",
            [
                $"The provided password for clock '{clockName}' is empty or is not a accepted user- or administrator password."
            ]);
        private static ErrorMessage StopReasonInvalidErrorMessage(string? reason) =>
            new(HttpStatusCode.BadRequest, ApiDocumentation, "StopReasonInvalid",
            [
                string.IsNullOrWhiteSpace(reason) ? "Stop reason is not provided." : $"Stop reason '{reason}' is invalid.",
                "Please, consult the documentation for the valid stop reasons."
            ]);

        private static ErrorMessage UserNameMissingErrorMessage(string? userName) =>
            new(HttpStatusCode.BadRequest, ApiDocumentation, "UserNameMissing",
            [
                string.IsNullOrWhiteSpace(userName) ? "User name is not provided." : $"User name '{userName}' is invalid.",
                "Please, consult the documentation for how to provide user name."
            ]);

        private static ErrorMessage SettingsErrorMessage(string? clockName) =>
             new(HttpStatusCode.BadRequest, ApiDocumentation, "SettingsError",
             [
                "API call payload does not contains valid settings or is empty.",
                string.IsNullOrWhiteSpace(clockName) ? "Clock name is not provided." : $"Clock name is '{clockName}'.",
                "Please, consult the documentation for how to provide correct settings."
             ]);

        private static ErrorMessage UserNameAlreadyTakenErrorMessage(string? user) =>
             new(HttpStatusCode.Conflict, ApiDocumentation, "UserNameAlreadyTaken",
             [
                string.IsNullOrWhiteSpace(user) ? "User name is not provided." : $"User name '{user}' is already taken.",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "Please, consult the documentation for how to create a clock with a new user name."
             ]);
    }
}
