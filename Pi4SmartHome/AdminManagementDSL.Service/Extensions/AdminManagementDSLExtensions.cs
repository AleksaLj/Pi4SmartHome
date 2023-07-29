using AdminManagementDSL.Service.Implementations;
using AdminManagementDSL.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdminManagementDSL.Service.Extensions
{
    public static class AdminManagementDSLExtensions
    {
        public static IServiceCollection AddAdminManagementDSLService(this IServiceCollection services)
        {
            services.AddSingleton<IAdminManagementDSLService, AdminManagementDSLService>();

            return services;
        }

        public static IAdminManagementDSLService GetAdminManagementDSLService(this IServiceProvider services)
        {
            return services.GetService<IAdminManagementDSLService>()!;
        }
    }
}
