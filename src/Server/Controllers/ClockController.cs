using Tellurian.Trains.MeetingApp.Server.Extensions;

namespace Tellurian.Trains.MeetingApp.Server.Controllers
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
        public ClockController(ClockServers servers)
        {
            Servers = servers;
        }

        private readonly ClockServers Servers;
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
        /// <param name="password">The clocks user- or administratior password.</param>
        /// <returns>Array of strings with user name, IP-address and last time cloclk was accessed.</returns>
        [SwaggerResponse((int)HttpStatusCode.OK, "Clock users was found.", typeof(IEnumerable<ClockUser>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Users")]
        public IActionResult Users(
        [SwaggerParameter("Clock name", Required = true)] string? clock,
        [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password)

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
        [SwaggerResponse((int)HttpStatusCode.OK, "Named clock was found.", typeof(ClockStatus))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/Time")]
        public IActionResult Time(
        [SwaggerParameter("Clock name", Required = true)] string clock,
        [FromQuery, SwaggerParameter("Username")] string? user,
        [FromQuery, SwaggerParameter("Client version number")] string? clientVersion)
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Settings for clock was found.", typeof(ClockSettings))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [Produces("application/json", "text/json")]
        [HttpGet("{clock}/settings")]
        public IActionResult Settings(
        [SwaggerParameter("Clock name", Required = true)] string clock,
        [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password)
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Clock was started")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpPut("{clock}/start")]
        public IActionResult Start(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
            [FromQuery, SwaggerParameter("User or administrator password", Required = true)] string? password)
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was stopped")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpPut("{clock}/stop")]
        public IActionResult Stop(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Reason for stopping clock", Required = true)] string? reason,
            [FromQuery, SwaggerParameter("User- or administrator password", Required = true)] string? password)
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
        [SwaggerResponse((int)HttpStatusCode.OK, "User was added as a clock user.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock administrator password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "User name is already taken.")]
        [HttpPut("{clock}/user")]
        public IActionResult ClockUser(
            [SwaggerParameter("Clock name", Required = true)] string? clock,
            [FromQuery, SwaggerParameter("User password", Required = true)] string? password,
            [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
            [FromQuery, SwaggerParameter("Client version", Required = true)] string? client)
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was created")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "Clock name is already taken.")]
        [HttpPost("create")]
        public IActionResult Create(
        [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
        [FromBody, SwaggerRequestBody("Clock settings", Required = true)] ClockSettings settings)
        {
            if (string.IsNullOrWhiteSpace(user)) return BadRequest(UserNameMissingErrorMessage(user));
            if (settings is null || string.IsNullOrWhiteSpace(settings.Name) || string.IsNullOrWhiteSpace(settings.AdministratorPassword)) return BadRequest(SettingsErrorMessage(nameof(settings)));
            var clock = settings.Name;
            var instance = Servers.Instance(clock);
            if (instance is not null) return Conflict(new ErrorMessage(HttpStatusCode.Conflict, ApiDocumentation, "ClockAlreadyExists", new[] { $"Clock name '{clock}' is already taken." }));
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Clocks was updated.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request has incomplete data, see error response for details.")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Not authorized, clock administrator password is not correct.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Named clock does not exist.")]
        [HttpPut("{clock}/settings")]
        public IActionResult Update(
           [SwaggerParameter("Clock name", Required = true)] string? clock,
           [FromQuery, SwaggerParameter("Username", Required = true)] string? user,
           [FromQuery, SwaggerParameter("Administrator password", Required = true)] string? password,
           [FromBody, SwaggerRequestBody("Clock settings", Required = true)] ClockSettings settings)
        {
            if (string.IsNullOrWhiteSpace(clock)) return BadRequest(ClockNameMissingErrorMessage());
            if (settings is null) return BadRequest(SettingsErrorMessage(clock));
            var instance = Servers.Instance(clock);
            if (instance is null) return NotFound(ClockNotFoundErrorMessage(clock));
            if (!instance.IsAdministrator(password)) return Unauthorized(UserUnauthorizedErrorMessage(clock));
            return instance.UpdateSettings(RemoteIpAddress, user, password, settings.AsSettings()) ? Ok() : Unauthorized(UserUnauthorizedErrorMessage(clock));
        }

        private static ErrorMessage ClockNameMissingErrorMessage() =>
           new(HttpStatusCode.NotFound, ApiDocumentation, "ClockNotFound", new[] {
                $"Clock name was not provided.",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "See documentation how to create a new clock."
           });
        private static ErrorMessage ClockNotFoundErrorMessage(string? clockName) =>
            new(HttpStatusCode.NotFound, ApiDocumentation, "ClockNotFound", new[] {
                $"Clock '{clockName}' does not exist. ",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "See documentation how to create a new clock."
            });
        private static ErrorMessage AdministratorUnauthorizedErrorMessage(string clockName) =>
            new(HttpStatusCode.Unauthorized, ApiDocumentation, "AdministratorUnauthorized", new[]
            {
                $"The provided password for clock '{clockName}' is empty or is an accepted the administrator password."
            });

        private static ErrorMessage UserUnauthorizedErrorMessage(string clockName) =>
            new(HttpStatusCode.Unauthorized, ApiDocumentation, "UserUnauthorized", new[]
            {
                $"The provided password for clock '{clockName}' is empty or is not a accepted user- or administrator password."
            });
        private static ErrorMessage StopReasonInvalidErrorMessage(string? reason) =>
            new(HttpStatusCode.BadRequest, ApiDocumentation, "StopReasonInvalid", new[]
            {
                string.IsNullOrWhiteSpace(reason) ? "Stop reason is not provided." : $"Stop reason '{reason}' is invalid.",
                "Please, consult the documentation for the valid stop reasons."
            });

        private static ErrorMessage UserNameMissingErrorMessage(string? userName) =>
            new(HttpStatusCode.BadRequest, ApiDocumentation, "UserNameMissing", new[]
            {
                string.IsNullOrWhiteSpace(userName) ? "User name is not provided." : $"User name '{userName}' is invalid.",
                "Please, consult the documentation for how to provide user name."
            });

        private static ErrorMessage SettingsErrorMessage(string? clockName) =>
             new(HttpStatusCode.BadRequest, ApiDocumentation, "SettingsError", new[]
             {
                "API call payload does not contains valid settings or is empty.",
                string.IsNullOrWhiteSpace(clockName) ? "Clock name is not provided." : $"Clock name is '{clockName}'.",
                "Please, consult the documentation for how to provide correct settings."
             });

        private static ErrorMessage UserNameAlreadyTakenErrorMessage(string? user) =>
             new(HttpStatusCode.Conflict, ApiDocumentation, "UserNameAlreadyTaken", new[]
             {
                string.IsNullOrWhiteSpace(user) ? "User name is not provided." : $"User name '{user}' is already taken.",
                "Use '/api/avaliable' to find which clocks that currently exists.",
                "Please, consult the documentation for how to create a clock with a new user name."
             });
    }
}
