using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;

namespace Tellurian.Trains.MeetingApp.Client.Extensions;

public static class WebAssemblyHostExtensions
{
    public async static Task SetDefaultLanguageAsync(this WebAssemblyHost host)
    {
        var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
        var result = await jsInterop.InvokeAsync<string>("preferredLanguage.get");
        if (string.IsNullOrWhiteSpace(result)) return;
        var culture = new CultureInfo(result);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
