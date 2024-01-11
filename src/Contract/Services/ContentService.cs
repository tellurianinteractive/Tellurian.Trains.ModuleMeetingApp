namespace Tellurian.Trains.MeetingApp.Contracts.Services;

public class ContentService(HttpClient client)
{
    private readonly HttpClient Client = client;

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
