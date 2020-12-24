using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Tellurian.Trains.Clocks.Server;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Tellurian.Trains.MeetingApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<ClockServerOptions>(Configuration.GetSection(nameof(ClockServerOptions)));
            services.AddSingleton<ClockServers>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Module Meeting App API",
                    Description = "API for getting and control the Fast Clock.",
                    Contact = new OpenApiContact { Name = "Stefan Fjällemark" },
                    License = new OpenApiLicense { Name = "GPL-3.0 Licence" }
                });
                c.IgnoreObsoleteProperties();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Tellurian.Trains.MeetingApp.Server.xml"), includeControllerXmlComments: true);
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSwagger(c => c.RouteTemplate = "openapi/{documentName}/openapi.json");
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "openapi";
                c.SwaggerEndpoint("/openapi/v2/openapi.json", "Version 2 documentation");
                c.DocumentTitle = "Tellurian Trains Module Meeting App Open API";
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
