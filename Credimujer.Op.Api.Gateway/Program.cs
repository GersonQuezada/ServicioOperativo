using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Credimujer.Op.Api.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var directory = System.IO.Directory.GetCurrentDirectory();
            //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.GetFullPath("pe-ferreyros-gcp-operapp.json", directory));
            var conf = GetConfig();
            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });

            builder.UseKestrel()

                //.UseContentRoot(Directory.GetCurrentDirectory())
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .UseStartup<Startup>()
                .UseUrls($"http://*:{conf.GetSection("Host:Port").Get<string>()}")
                //.ConfigureServices(services =>
                //    //services.AddAutofac()
                //)
                ;

            var host = builder.Build();
            host.Run();
        }

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
    }
}