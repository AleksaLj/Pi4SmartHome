using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Core.Configurations;
using AdminManagementDSL.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminManagementDSL.Infrastructure.Common.Extensions
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddUsersRepo(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepo, UsersRepo>();

            return services;
        }

        public static IServiceCollection AddDevicesRepo(this IServiceCollection services)
        {
            services.AddScoped<IDevicesRepo, DevicesRepo>();

            return services;
        }

        public static IServiceCollection AddEstatesRepo(this IServiceCollection services)
        {
            services.AddScoped<IEstatesRepo, EstatesRepo>();

            return services;
        }

        public static IServiceCollection AddEstatePartRepo(this IServiceCollection services)
        {
            services.AddScoped<IEstatePartRepo, EstatePartRepo>();

            return services;
        }

        public static IServiceCollection AddSqlConnOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SqlConnectionOptions>().Bind(configuration.GetSection(Configs.SqlConfigRoot));

            return services;
        }

        public static IUsersRepo? GetUsersRepo(this IServiceProvider provider)
        {
            return provider.GetService<IUsersRepo>();
        }

        public static IDevicesRepo? GetDevicesRepo(this IServiceProvider provider)
        {
            return provider.GetService<IDevicesRepo>();
        }

        public static IEstatesRepo? GetEstatesRepo(this IServiceProvider provider)
        {
            return provider.GetService<IEstatesRepo>();
        }

        public static IEstatePartRepo? GetEstatePartRepo(this IServiceProvider provider)
        {
            return provider.GetService<IEstatePartRepo>();
        }

        public static SqlConnectionOptions? GetSqlConnOptions(this IConfiguration configuration)
        {
            return configuration.GetSection(Configs.SqlConfigRoot).Get<SqlConnectionOptions>();
        }
    }
}
