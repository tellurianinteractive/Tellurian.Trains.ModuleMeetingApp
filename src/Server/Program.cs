var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<ClockServerOptions>(builder.Configuration.GetSection(nameof(ClockServerOptions)));
builder.Services.AddSingleton<ITimeProvider, SystemTimeProvider>();
builder.Services.AddSingleton<LanguageService>();
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
    app.UseHsts();
}

app.UseSwagger(c => c.RouteTemplate = "openapi/{documentName}/openapi.json");
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "openapi";
    c.SwaggerEndpoint("/openapi/v3/openapi.json", "Version 3 documentation");
    c.DocumentTitle = "Tellurian Trains Module Meeting App Open API";
});

app.UseRequestLocalization(options =>
{
    options.SetDefaultCulture(LanguageService.DefaultLanguage);
    options.AddSupportedCultures(LanguageService.Languages);
    options.AddSupportedUICultures(LanguageService.Languages);
    options.FallBackToParentCultures = true;
    options.FallBackToParentUICultures = true;
});

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Logger.LogInformation("App version {version} starting at {time}", Assembly.GetExecutingAssembly().GetName().Version, DateTimeOffset.Now.ToString("g"));

app.Run();
