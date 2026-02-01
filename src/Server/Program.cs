using Microsoft.OpenApi;
using System.Reflection;
using Tellurian.Trains.MeetingApp.Clocks;
using Tellurian.Trains.MeetingApp.Clocks.Implementations;
using Tellurian.Trains.MeetingApp.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<ClockServerOptions>(builder.Configuration.GetSection(nameof(ClockServerOptions)));
builder.Services.AddSingleton<ITimeProvider, SystemTimeProvider>();
builder.Services.AddSingleton<LanguageUtility>();
builder.Services.AddSingleton<ClockServers>();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "3.0";
        document.Info.Title = "Fastclock API";
        document.Info.Description = "API for accessing fast clock functionality.";
        document.Info.TermsOfService = new Uri("https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/Terms-of-Use");
        document.Info.Contact = new OpenApiContact
        {
            Name = "Stefan Fjällemark",
            Email = "stefan@tellurian.se",
            Url = new Uri("https://github.com/tellurianinteractive/Tellurian.Trains.ModulesRegistryApp")
        };
        document.Info.License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();
var httpsDisabled = app.Configuration.GetValue("DisableHttps", false);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    if (!httpsDisabled) app.UseHsts();
}
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapOpenApi("openapi/{document}/openapi.json");
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("openapi/v3/openapi.json", "Version 3 documentation");
    c.DocumentTitle = "Tellurian Trains Module Meeting App Open API";
});

app.UseRequestLocalization(options =>
{
    options.SetDefaultCulture(LanguageUtility.DefaultLanguage);
    options.AddSupportedCultures(LanguageUtility.Languages);
    options.AddSupportedUICultures(LanguageUtility.Languages);
    options.FallBackToParentCultures = true;
    options.FallBackToParentUICultures = true;
});

if (!httpsDisabled) app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapStaticAssets();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Logger.LogInformation("App version {version} starting at {time}", Assembly.GetExecutingAssembly().GetName().Version, DateTimeOffset.Now.ToString("g"));

app.Run();
