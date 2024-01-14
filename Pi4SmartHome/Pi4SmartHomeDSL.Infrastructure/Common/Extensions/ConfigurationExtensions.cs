using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pi4SmartHomeDSL.Core.Configurations;

namespace Pi4SmartHomeDSL.Infrastructure.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddIoTHubConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<IoTHubConnectionOptions>().Bind(configuration.GetSection(Configs.IoTHubRoot));

            return services;
        }
    }
}
