using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common;
using Credimujer.Op.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Credimujer.Op.Api.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string appSettingName = string.Empty;
            if (environmentName == "Production")
                appSettingName = ".Production";

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath)
               .AddJsonFile($"appsettings{appSettingName}.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"configuration{appSettingName}.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSetting>(appSettingsSection);
            services.AddSingleton(cfg => cfg.GetService<IOptions<AppSetting>>().Value);

            services.AddAppSettingExtesion(Configuration);
            //services.AddCache(Configuration);
            services.AddSwaggerExtesion("API Gateway");

            var appSettings = appSettingsSection.Get<AppSetting>();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.JWTConfigurations.Secret));

            //services.AddLoggingExtension(new LoggingOptions
            //{
            //    AccountService = Path.GetFullPath("pe-ferreyros-gcp-lms.json", Directory.GetCurrentDirectory()),
            //    LogName = Assembly.GetEntryAssembly().GetName().Name,
            //    ProjectId = appSettings.GoogleResource.Logging.ProjectId
            //});

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = appSettings.JWTConfigurations.Iss,
                ValidateAudience = true,
                ValidAudience = appSettings.JWTConfigurations.Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            var tokenValidationParameters1 = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = "www.CrediMujer.com.pe",
                ValidateAudience = true,
                ValidAudience = "CrediMujer",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            services.AddAuthentication()
                .AddJwtBearer(AuthenticateScheme.Security, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = tokenValidationParameters1;
                });

            services.AddAuthentication()
                .AddJwtBearer(AuthenticateScheme.PersonalOperativo, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOcelot(Configuration);
            services.AddSwaggerForOcelot(Configuration);
            services.AddControllers();

            services.AddSwaggerGen(cfg =>
            {
                cfg.DocumentFilter<SwaggerExtension.HideOcelotControllersFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string pathBase = Configuration.GetSection("Host:PathBase").Get<string>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()
            );

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); ;
            });

            app.UseSwagger();
            app.UseSwaggerForOcelotUI(c =>
            {
                c.DownstreamSwaggerEndPointBasePath = $"{pathBase}/swagger/docs";
                c.PathToSwaggerGenerator = $"{pathBase}/swagger/docs";
                c.InjectStylesheet($"{pathBase}/swagger/header.css");
                c.DocumentTitle = "LMS Api Operación Gateway";
            });
            app.Use((context, next) =>
            {
                context.Request.PathBase = new PathString(pathBase);
                return next();
            });
            await app.UseOcelot();
        }
    }
}