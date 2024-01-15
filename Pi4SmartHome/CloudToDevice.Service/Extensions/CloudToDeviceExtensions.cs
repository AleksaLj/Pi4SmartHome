using CloudToDevice.Service.Implementations;
using CloudToDevice.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CloudToDevice.Service.Extensions
{
    public static class CloudToDeviceExtensions
    {
        public static IServiceCollection AddCloudToDeviceMessageHandler(this IServiceCollection services)
        {
            services.AddSingleton<ICloudToDeviceMessageHandler, CloudToDeviceMessageHandler>();

            return services;
        }

        public static IServiceCollection AddCloudToDeviceService(this IServiceCollection services)
        {
            services.AddSingleton<ICloudToDeviceService, CloudToDeviceService>();

            return services;
        }

        public static ICloudToDeviceService GetCloudToDeviceService(this IServiceProvider services)
        {
            return services.GetService<ICloudToDeviceService>()!;
        }
    }
}
