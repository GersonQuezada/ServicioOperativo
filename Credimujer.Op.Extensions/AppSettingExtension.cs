using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Credimujer.Op.Extensions
{
    public static class AppSettingExtension
    {
        public static void AddAppSettingExtesion(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSetting>(appSettingsSection);
            services.AddSingleton(cfg => cfg.GetService<IOptions<AppSetting>>().Value);
        }
    }
}