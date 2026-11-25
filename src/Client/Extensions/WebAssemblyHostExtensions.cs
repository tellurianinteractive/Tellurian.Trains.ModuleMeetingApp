using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Tellurian.Trains.MeetingApp.Client.Extensions;

public static class WebAssemblyHostExtensions
{
    public async static Task SetDefaultLanguageAsync(this WebAssemblyHost host)
    {
        var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
        string? result;
        try
        {
            result = await jsInterop.InvokeAsync<string?>("preferredLanguage.get");
        }
        catch (JSException)
        {
            return; // Use browser default language when the stored preference cannot be read.
        }
        if (string.IsNullOrWhiteSpace(result)) return;
        CultureInfo culture;
        try
        {
            culture = new CultureInfo(result);
        }
        catch (CultureNotFoundException)
        {
            return;
        }
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
