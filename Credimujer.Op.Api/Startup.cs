using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using FluentValidation.AspNetCore;
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
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Credimujer.Op.Common;
using Credimujer.Op.Extensions;
using Credimujer.Op.Repository.Implementations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyModel;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces.Data;
using Credimujer.Op.Service.Implementations.Base;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;
using Credimujer.Op.Api.Filter;

namespace Credimujer.Op.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public IServiceCollection services { get; private set; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppSettingExtesion(Configuration);
            services.AddJWTExtesion(Configuration, AuthenticateScheme.PersonalOperativo);
            services.AddSwaggerExtesion("CrediMujer Operativo Personal");
            services.AddControllers(c => c.Filters.Add<ValidationFilter>())
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                ).AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());
            ;

            //esto es para consultar un servicio Externo
            services.AddHttpClient<HttpClientService>().ConfigureHttpMessageHandlerBuilder(builder =>
            {
                builder.PrimaryHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
                };
            });
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("OperativoPersonalv1", new OpenApiInfo { Title = "Credimujer.Op.Api", Version = "v1" });
                cfg.CustomSchemaIds(type => type.ToString());
                cfg.DocumentFilter<SwaggerExtension.HideOcelotControllersFilter>();
            });
            this.services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Credimujer.Op.Api v1"));
            }
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseCache();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                c.InjectStylesheet("/swagger/header.css");
                c.DocumentTitle = "Credimujer.Op.Api";
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (library.Name.StartsWith("Credimujer"))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }

            var assembliesArray = assemblies.ToArray();

            builder.RegisterAssemblyTypes(assembliesArray).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterAssemblyTypes(assembliesArray).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterAssemblyTypes(assembliesArray).Where(t => t.Name.EndsWith("Application")).AsImplementedInterfaces().InstancePerLifetimeScope();

            Autofac.IContainer container = null;
            builder.Register(c => container).AsSelf().SingleInstance();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            //builder.RegisterType<Logger>()
            //    .As<ILoggerApplication>()
            //    .WithParameter(new NamedParameter("projectName", System.Reflection.Assembly.GetEntryAssembly().GetName().Name))
            //    .SingleInstance(); ;
        }
    }
}