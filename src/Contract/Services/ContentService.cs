namespace Tellurian.Trains.MeetingApp.Contracts.Services;

public class ContentService
{
    public ContentService(HttpClient client) => Client = client;

    private readonly HttpClient Client;

    public async Task<string> GetHtmlContentAsync(CultureInfo culture, string id)
    {
        try
        {
            return await Client.GetStringAsync($"api/content/{culture.TwoLetterISOLanguageName}/{id}");
        }
        catch (HttpRequestException)
        {
            return string.Empty;
        }
    }
}
