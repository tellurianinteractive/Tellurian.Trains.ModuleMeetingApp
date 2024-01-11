
namespace Tellurian.Trains.MeetingApp.Contracts.Services;


public interface IClockService
{
    Task<IEnumerable<string>> AvailableClocksAsync();
    Task<ClockStatus?> GetStatusAsync(string clockName, string? userName, string? clientVersion);

}
public interface IClockAdministratorService : IClockService
{
    Task<HttpResponseMessage> CreateAsync(string? userName, ClockSettings settings);
    Task<IEnumerable<ClockUser>?> GetCurrentUsersAsync(string? clockName, string? administratorPassword);
    Task<ClockSettings?> GetSettingsAsync(string? clockName, string? administratorPassword);
    Task<HttpResponseMessage> StartAsync(string clockName, string? clockPassword, string? userName);
    Task<HttpResponseMessage> StopAsync(string clockName, string? clockPassword, string? userName, string stopReason);
    Task<HttpResponseMessage> UpdateAsync(string clockName, string? userName, string? administratorPassword, ClockSettings settings);
    Task<HttpResponseMessage> UpdateUserAsync(string clockName, string? clockPassword, string? userName, string? clientVersionNumber);
}