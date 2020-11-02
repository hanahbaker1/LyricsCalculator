using System.Net.Http.Headers;
using HealthChecks.UI.Client;
using LyricsCalculator.Api.Configuration;
using LyricsCalculator.Common;
using LyricsCalculator.Processor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LyricsCalculator.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private static readonly AppConfig AppConfig = new AppConfig();

        public Startup(IConfiguration configuration)
        {
            _configuration = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .Build();

            _configuration.Bind(AppConfig);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHttpContextAccessor();

            services.AddScoped<ISearchRepository, SearchRepository>();
            ConfigureHttpClients(services);

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LyricsWordCounter", Version = "v1"});
            });
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            services.AddHttpClient<ISongsClient, MusicBrainzSongsClient>()
                .ConfigureHttpClient(o =>
                {
                    o.BaseAddress = AppConfig.MusicBrainzSongUri.GuaranteeEndingSlash();
                    o.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    o.DefaultRequestHeaders.Add("User-Agent","LyricsWordCounter");
                });

            services.AddHttpClient<ILyricsClient, LyricsOvhClient>()
                .ConfigureHttpClient(o =>
                {
                    o.BaseAddress = AppConfig.OvhLyricsUri.GuaranteeEndingSlash();
                    o.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(a => a.SwaggerEndpoint("../swagger/v1/swagger.json", "LyricsWordCounter"));
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}