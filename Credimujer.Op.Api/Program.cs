using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace Credimujer.Op.Api
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
        //public static void Main(string[] args)
        //{
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseStartup<Startup>()
        //        .ConfigureServices(services => services.AddAutofac())
        //        .Build()
        //        .Run();
        //}
        public static void Main(string[] args)
        {
            var conf = GetConfig();
            CreateHostBuilder(args, conf).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot conf) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls($"http://*:{conf.GetSection("Host:Port").Get<string>()}");
                    //webBuilder.ConfigureServices(services => services.AddAutofac());
                    //webBuilder.Build();
                    //webBuilder.Start();
                });

        private static IConfigurationRoot GetConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string appSettingName = string.Empty;
            if (env == "Production")
                appSettingName = ".Production";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings{appSettingName}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build(); ;
        }
    }
}