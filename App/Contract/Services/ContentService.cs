using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tellurian.Trains.MeetingApp.Contract.Services
{
    public class ContentService
    {
        public ContentService(HttpClient client) => Client = client;
 
        private readonly HttpClient Client;

        public async Task<string> GetHtmlContent(CultureInfo culture, string id) =>
            await Client.GetStringAsync($"api/content/{culture.TwoLetterISOLanguageName}/{id}");
    }
}
