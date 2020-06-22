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
            await Http.GetFromJsonAsync<IEnumerable<string>>("api/clock/availableclocks").ConfigureAwait(false);

        public async Task<ClockStatus> GetStatus(string clockName, string? userName)
        {
            try
            {
                return await Http.GetFromJsonAsync<ClockStatus>($"api/Clock/Time/{clockName}?user={userName}").ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                return new ClockStatus { IsUnavailable = true };
            }
        }

        public async Task<HttpResponseMessage> Start(string clockName, string? clockPassword, string? userName) =>
            await Http.GetAsync($"api/Clock/Start/{clockName}?apiKey={ApiKey}&user={userName}&password={clockPassword}").ConfigureAwait(false);

        public async Task<HttpResponseMessage> Stop(string clockName, string? clockPassword, string? userName, string stopReason) =>
            await Http.GetAsync($"api/Clock/Stop/{clockName}?apiKey={ApiKey}&user={userName}&password={clockPassword}&reason={stopReason}").ConfigureAwait(false);

        public async Task<HttpResponseMessage> Update(string clockName, string? userName, ClockSettings settings) =>
            await Http.PostAsJsonAsync($"api/Clock/UpdateSettings/{clockName}?apiKey={ApiKey}&user={userName}", settings).ConfigureAwait(false);

        public async Task<ClockSettings> GetSettings(string clockName) =>
            await Http.GetFromJsonAsync<ClockSettings>($"api/Clock/Settings/{clockName}").ConfigureAwait(false);
    }
}
