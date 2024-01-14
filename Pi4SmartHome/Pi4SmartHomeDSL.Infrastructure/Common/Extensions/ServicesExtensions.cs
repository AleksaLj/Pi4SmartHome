using Microsoft.Extensions.DependencyInjection;
using Pi4SmartHomeDSL.Application.Interfaces;
using Pi4SmartHomeDSL.Infrastructure.CloudToDeviceService;

namespace Pi4SmartHomeDSL.Infrastructure.Common.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDeviceMessagingService(this IServiceCollection services)
        {
            services.AddTransient<IIoTHubCloudToDeviceMessagingService, IoTHubCloudToDeviceMessagingService>();

            return services;
        }

        public static IIoTHubCloudToDeviceMessagingService GetDeviceMessagingService(this IServiceProvider services)
        {
            return services.GetService<IIoTHubCloudToDeviceMessagingService>()!;
        }
    }
}
