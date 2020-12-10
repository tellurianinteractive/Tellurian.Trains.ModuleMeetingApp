using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tellurian.Trains.MeetingApp.Contract;

namespace Tellurian.Trains.MeetingApp.Client.Services
{
    public class RegistrationsService
    {
        public RegistrationsService(ILocalStorageService localStorage)
        {
            LocalStorage = localStorage;
        }
        private readonly ILocalStorageService LocalStorage;

        public async Task<bool> Set(Registration registration)
        {
            if (registration is null) return false;
            registration.IsInstructionVisible = false;
            await LocalStorage.SetItemAsync(Registration.Key, registration).ConfigureAwait(false);
            return true;
        }

        public async Task<Registration> Get()
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
    }
}
