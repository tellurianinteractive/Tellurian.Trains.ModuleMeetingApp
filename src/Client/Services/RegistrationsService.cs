using Blazored.LocalStorage;
using System.Text.Json;
using Tellurian.Trains.MeetingApp.Client.Model;
using Tellurian.Trains.MeetingApp.Contracts;

namespace Tellurian.Trains.MeetingApp.Client.Services
{
    public class RegistrationsService
    {
        public RegistrationsService(ILocalStorageService localStorage)
        {
            LocalStorage = localStorage;
        }
        private readonly ILocalStorageService LocalStorage;

        public async Task<bool> SetAsync(Registration registration)
        {
            if (registration is null) return false;
            registration.IsInstructionVisible = false;
            await LocalStorage.SetItemAsync(Registration.Key, registration).ConfigureAwait(false);
            return true;
        }

        public async Task<Registration> GetAsync()
        {
            if (await LocalStorage.ContainKeyAsync(Registration.Key).ConfigureAwait(false))
            {
                try
                {
                    return await LocalStorage.GetItemAsync<Registration>(Registration.Key).ConfigureAwait(false);
                }
                catch (JsonException)
                {
                    await LocalStorage.RemoveItemAsync(Registration.Key).ConfigureAwait(false);
                }
            }
            return new Registration();
        }

        public async Task<Registration> UseAvailableClockOnlyAsync(IEnumerable<string>? availableClocks)
        {
            var registration = await GetAsync();
            if (availableClocks is null || !availableClocks.Contains(registration.ClockName, StringComparer.OrdinalIgnoreCase))
            {
                registration.ClockName = ClockSettings.DemoClockName;
                registration.ClockPassword = ClockSettings.DemoClockPassword;
                await SetAsync(registration);
            }
            return registration;
        }
    }
}