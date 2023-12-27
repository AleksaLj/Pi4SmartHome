using AdminManagementDSL.AdminDSL.Common.Interfaces;
using AdminManagementDSL.AdminDSL.Interpreter;
using AdminManagementDSL.AdminDSL.Interpreter.Visitor;
using AdminManagementDSL.AdminDSL.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace AdminManagementDSL.AdminDSL.Common.Extensions
{
    public static class AdminDSLExtensions
    {
        public static IServiceCollection AddAdminDSLParser(this IServiceCollection services)
        {
            services.AddScoped<IAdminDSLParser, AdminDSLParser>();

            return services;
        }

        public static IServiceCollection AddNodeVisitor(this IServiceCollection services)
        {
            services.AddScoped<INodeVisitor, NodeVisitor>();

            return services;
        }

        public static IServiceCollection AddAdminDSLInterpreter(this IServiceCollection services)
        {
            services.AddScoped<IAdminDSLInterpreter, AdminDSLInterpreter>();

            return services;
        }

        public static IAdminDSLParser? GetAdminDSLParser(this IServiceProvider services)
        {
            return services.GetService<IAdminDSLParser>();
        }

        public static IAdminDSLInterpreter? GetAdminDSLInterpreter(this IServiceProvider services)
        {
            return services.GetService<IAdminDSLInterpreter>();
        }
    }
}
