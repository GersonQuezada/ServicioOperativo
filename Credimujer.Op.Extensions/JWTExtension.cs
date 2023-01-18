using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Credimujer.Op.Extensions
{
    public static class JWTExtension
    {
        public static void AddJWTExtesion(this IServiceCollection services, IConfiguration configuration, string authenticateScheme)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSetting>();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.JWTConfigurations.Secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.FromMinutes(0),

                RequireExpirationTime = true,
                RequireSignedTokens = true,

                ValidateActor = false,
                ValidateAudience = false,
                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = appSettings.JWTConfigurations.Iss,
                ValidAudience = appSettings.JWTConfigurations.Aud,
                IssuerSigningKey = signingKey,

            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = authenticateScheme;
                    o.DefaultChallengeScheme = authenticateScheme;
                })
                .AddJwtBearer(authenticateScheme, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = tokenValidationParameters;
                    x.SaveToken = true;
                });

        }
    }
}
