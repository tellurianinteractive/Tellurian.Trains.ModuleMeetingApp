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

        public async Task<IEnumerable<ClockUser>> Users(string? clockName, string? clockPassword)
        {
            if (string.IsNullOrEmpty(clockName)) return Array.Empty<ClockUser>();
            try
            {
                return await Http.GetFromJsonAsync<IEnumerable<ClockUser>>($"api/clocks/{clockName}/users?apiKey={ApiKey}&password={clockPassword}").ConfigureAwait(false);
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
                return await Http.GetFromJsonAsync<ClockStatus>($"api/clocks/{clockName}/Time?user={userName}").ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return new ClockStatus { IsUnavailable = true };
            }
        }

        public async Task<HttpResponseMessage> Start(string clockName, string? clockPassword, string? userName) =>
            await Http.PutAsync($"api/clocks/{clockName}/Start?apiKey={ApiKey}&user={userName}&password={clockPassword}", null).ConfigureAwait(false);

        public async Task<HttpResponseMessage> Stop(string clockName, string? clockPassword, string? userName, string stopReason) =>
            await Http.PutAsync($"api/clocks/{clockName}/Stop?apiKey={ApiKey}&user={userName}&password={clockPassword}&reason={stopReason}", null).ConfigureAwait(false);

        public async Task<HttpResponseMessage> Update(string clockName, string? userName, ClockSettings settings) =>
            await Http.PostAsJsonAsync($"api/clocks/{clockName}/Update?apiKey={ApiKey}&user={userName}", settings).ConfigureAwait(false);

        public async Task<ClockSettings> GetSettings(string clockName) =>
            await Http.GetFromJsonAsync<ClockSettings>($"api/clocks/{clockName}/Settings").ConfigureAwait(false);
    }
}
