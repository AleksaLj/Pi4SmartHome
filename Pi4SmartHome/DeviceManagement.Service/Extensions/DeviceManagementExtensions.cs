using DeviceManagement.Service.Implementations;
using DeviceManagement.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagement.Service.Extensions
{
    public static class DeviceManagementExtensions
    {
        public static IServiceCollection AddDeviceManagementService(this IServiceCollection services)
        {
            services.AddTransient<IDeviceManagementService, DeviceManagementService>();

            return services;
        }

        public static IServiceCollection AddAdminDslInterpreterEndMsgHandler(this IServiceCollection services)
        {
            services.AddTransient<IAdminDSLInterpreterEndMsgHandler, AdminDSLInterpreterEndMsgHandler>();

            return services;
        }

        public static IDeviceManagementService GetDeviceManagementService(this IServiceProvider services)
        {
            return services.GetService<IDeviceManagementService>()!;
        }
    }
}
