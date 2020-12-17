using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tellurian.Trains.MeetingApp.Contract.Services
{
    public class LanguageService
    {
        public LanguageService(HttpClient client) => Client = client;
 
        private readonly HttpClient Client;

        public async Task<string> GetHtmlContent(CultureInfo culture, string contentName) =>
            await Client.GetStringAsync($"api/languages/{culture.TwoLetterISOLanguageName}/{contentName}");
    }
}
