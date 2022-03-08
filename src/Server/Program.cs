using Microsoft.OpenApi.Models;
using Tellurian.Trains.MeetingApp.Clocks.Implementations;
using Tellurian.Trains.MeetingApp.Clocks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<ClockServerOptions>(builder.Configuration.GetSection(nameof(ClockServerOptions)));
builder.Services.AddSingleton<ClockServers>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v3", new OpenApiInfo
    {
        Version = "v3",
        Title = "Module Meeting App API",
        Description = "API for getting and control the Fast Clock.",
        Contact = new OpenApiContact { Name = "Stefan Fjällemark" },
        License = new OpenApiLicense { Name = "GPL-3.0 Licence" }
    });
    c.IgnoreObsoleteProperties();
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Tellurian.Trains.MeetingApp.Server.xml"), includeControllerXmlComments: true);
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseSwagger(c => c.RouteTemplate = "openapi/{documentName}/openapi.json");
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "openapi";
    c.SwaggerEndpoint("/openapi/v3/openapi.json", "Version 3 documentation");
    c.DocumentTitle = "Tellurian Trains Module Meeting App Open API";
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
