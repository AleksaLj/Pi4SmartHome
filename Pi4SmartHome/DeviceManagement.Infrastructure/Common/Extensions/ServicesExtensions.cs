using DeviceManagement.Application.Interfaces;
using DeviceManagement.Infrastructure.IoTHubDeviceService;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagement.Infrastructure.Common.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDeviceProvisioningService(this IServiceCollection services)
        {
            services.AddTransient<IIoTHubDeviceProvisioningService, IoTHubDeviceProvisioningService>();

            return services;
        }

        public static IServiceCollection AddDeviceMessagingService(this IServiceCollection services)
        {
            services.AddTransient<IIoTHubCloudToDeviceMessagingService, IoTHubCloudToDeviceMessagingService>();

            return services;
        }
    }
}
