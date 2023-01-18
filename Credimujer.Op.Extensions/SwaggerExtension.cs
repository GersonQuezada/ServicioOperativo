using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Credimujer.Op.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwaggerExtesion(this IServiceCollection services, string jwtTitle)
        {
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<HideOcelotControllersFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = jwtTitle, Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public class HideOcelotControllersFilter : IDocumentFilter
        {
            private static readonly string[] _ignoredPaths = { "/configuration", "/outputcache/{region}" };

            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                foreach (var ignorePath in _ignoredPaths)
                {
                    swaggerDoc.Paths.Remove(ignorePath);
                }
            }

        }
    }
}
