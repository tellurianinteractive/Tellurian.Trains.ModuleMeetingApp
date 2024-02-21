using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Tellurian.Trains.MeetingApp.Contracts.Services;

public class ClocksService(HttpClient http, ILogger<ClocksService> logger) : IClockAdministratorService
{
    private readonly HttpClient Http = http;
    private readonly ILogger<ClocksService> Logger = logger;

    public async Task<string?> GetServerVersionAsync()
    {
        try
        {
            var serverVersion = await Http.GetStringAsync("api/clocks/version").ConfigureAwait(false);
            return serverVersion;
        }
        catch (Exception ex)
        {
            Logger.LogError("Get version failed: {Error}", ex.Message);
            return string.Empty;
        }
    }

    public async Task<IEnumerable<string>> AvailableClocksAsync()
    {
        try
        {
            var clockNames = await Http.GetFromJsonAsync<IEnumerable<string>>("api/clocks/available").ConfigureAwait(false);
            if (clockNames == null || !clockNames.Any()) return [ClockSettings.DemoClockName];
            return clockNames;
        }
        catch (Exception)
        {
            return [ClockSettings.DemoClockName];
        }
    }

    public async Task<IEnumerable<ClockUser>?> GetCurrentUsersAsync(string? clockName, string? administratorPassword)
    {
        if (string.IsNullOrEmpty(clockName)) return [];
        try
        {
            return await Http.GetFromJsonAsync<IEnumerable<ClockUser>>($"api/clocks/{clockName}/users?password={administratorPassword}").ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            return [];
        }
    }

    public async Task<ClockStatus?> GetStatusAsync(string clockName, string? userName, string? clientVersion)
    {
        try
        {
            return await Http.GetFromJsonAsync<ClockStatus>($"api/clocks/{clockName}/Time?user={userName}&client={clientVersion}").ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            return new ClockStatus { Name = clockName, IsUnavailable = true };
        }
    }

    public async Task<HttpResponseMessage> StartAsync(string clockName, string? clockPassword, string? userName) =>
        await Http.PutAsync($"api/clocks/{clockName}/start?user={userName}&password={clockPassword}", null).ConfigureAwait(false);

    public async Task<HttpResponseMessage> StopAsync(string clockName, string? clockPassword, string? userName, string stopReason) =>
        await Http.PutAsync($"api/clocks/{clockName}/stop?user={userName}&password={clockPassword}&reason={stopReason}", null).ConfigureAwait(false);

    public async Task<HttpResponseMessage> UpdateUserAsync(string clockName, string? clockPassword, string? userName, string? clientVersionNumber) =>
        await Http.PutAsync($"api/clocks/{clockName}/user?user={userName}&password={clockPassword}&client={clientVersionNumber}", null).ConfigureAwait(false);

    public async Task<HttpResponseMessage> UpdateAsync(string clockName, string? userName, string? administratorPassword, ClockSettings settings) =>
        await Http.PutAsJsonAsync($"api/clocks/{clockName}/settings?user={userName}&password={administratorPassword}", settings).ConfigureAwait(false);
    public async Task<HttpResponseMessage> CreateAsync(string? userName, ClockSettings settings)
    {
        return await Http.PostAsJsonAsync($"api/clocks/create?user={userName}", settings).ConfigureAwait(false);
    }

    public async Task<ClockSettings?> GetSettingsAsync(string? clockName, string? administratorPassword)
    {
        try
        {
            var response = await Http.GetAsync($"api/clocks/{clockName}/settings?password={administratorPassword}").ConfigureAwait(false);
            return await response.GetSettingsAsync();
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
public static class ClockServiceExtensions
{
    public static async Task<ClockSettings?> GetSettingsAsync(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ClockSettings?>().ConfigureAwait(false);
        }
        return null;
    }

    public static async Task<string> GetErrorMessagesAsync(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorMessage>().ConfigureAwait(false);
            if (error is not null)
            {
                return string.Join(", ", error.Messages);
            }
        }
        return string.Empty;
    }
}
