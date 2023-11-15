using DeviceManagement.Application.Interfaces;
using DeviceManagement.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagement.Infrastructure.Common.Extensions
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddIoTDeviceRepo(this IServiceCollection services)
        {
            services.AddTransient<IIoTDeviceRepo, IoTDeviceRepo>();

            return services;
        }
    }
}
