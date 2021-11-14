global using Tellurian.Trains.MeetingApp.Contract;

using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tellurian.Trains.MeetingApp.Client;
using Tellurian.Trains.MeetingApp.Client.Services;
using Tellurian.Trains.MeetingApp.Contract.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalization( options => options.ResourcesPath = "Resources" );
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();

builder.Services.AddScoped<ContentService>();
builder.Services.AddScoped<RegistrationsService>();
builder.Services.AddScoped<ClocksService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var host = builder.Build();

await host.RunAsync();
