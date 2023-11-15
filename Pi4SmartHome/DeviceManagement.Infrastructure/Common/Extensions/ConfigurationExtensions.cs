using DeviceManagement.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagement.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddSqlConnOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SqlConnectionOptions>().Bind(configuration.GetSection(Configs.SqlConfigRoot));

            return services;
        }

        public static IServiceCollection AddIoTHubConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<IoTHubConnectionOptions>().Bind(configuration.GetSection(Configs.IoTHubRoot));

            return services;
        }
    }
}
