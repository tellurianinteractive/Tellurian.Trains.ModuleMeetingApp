using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tellurian.Trains.MeetingApp.Shared;

namespace Tellurian.Trains.MeetingApp.Client.Services
{
    public class ClocksService
    {
        public ClocksService(HttpClient http)
        {
            Http = http;
        }

        private readonly HttpClient Http;
        private static string ApiKey => ClockSettings.ClockApiKey;

        public async Task<IEnumerable<string>> AvailableClocks() =>
            await Http.GetFromJsonAsync<IEnumerable<string>>("api/clocks/available").ConfigureAwait(false);

        public async Task<IEnumerable<ClockUser>> Users(string? clockName, string? administratorPassword)
        {
            if (string.IsNullOrEmpty(clockName)) return Array.Empty<ClockUser>();
            try
            {
                return await Http.GetFromJsonAsync<IEnumerable<ClockUser>>($"api/clocks/{clockName}/users?apiKey={ApiKey}&password={administratorPassword}").ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return Array.Empty<ClockUser>();
            }
        }

        public async Task<ClockStatus> GetStatus(string clockName, string? userName)
        {
            try
            {
                return await Http.GetFromJsonAsync<ClockStatus>($"api/clocks/{clockName}/Time?user={userName}&client={ClockStatusExtension.ClientVersionNumber}").ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return new ClockStatus { IsUnavailable = true };
            }
        }

        public async Task<HttpResponseMessage> Start(string clockName, string? clockPassword, string? userName) =>
            await Http.PutAsync($"api/clocks/{clockName}/start?apiKey={ApiKey}&user={userName}&password={clockPassword}", null).ConfigureAwait(false);

        public async Task<HttpResponseMessage> Stop(string clockName, string? clockPassword, string? userName, string stopReason) =>
            await Http.PutAsync($"api/clocks/{clockName}/stop?apiKey={ApiKey}&user={userName}&password={clockPassword}&reason={stopReason}", null).ConfigureAwait(false);

        public async Task<HttpResponseMessage> User(string clockName, string? userName) =>
            await Http.PutAsync($"api/clocks/{clockName}/user?apiKey={ApiKey}&user={userName}&client={ClockStatusExtension.ClientVersionNumber}", null).ConfigureAwait(false);

        public async Task<HttpResponseMessage> Update(string clockName, string? userName, string? administratorPassword,  ClockSettings settings) =>
            await Http.PostAsJsonAsync($"api/clocks/{clockName}/update?apiKey={ApiKey}&user={userName}&password={administratorPassword}", settings).ConfigureAwait(false);

        public async Task<ClockSettings> GetSettings(string clockName) =>
            await Http.GetFromJsonAsync<ClockSettings>($"api/clocks/{clockName}/Settings").ConfigureAwait(false);
    }
}
