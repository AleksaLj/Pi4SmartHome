using Microsoft.Extensions.DependencyInjection;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;
using Pi4SmartHomeDSL.DSL.Parser;
using Pi4SmartHomeDSL.DSL.Scanner;

namespace Pi4SmartHomeDSL.DSL.Common.Extensions
{
    public static class Pi4SmartHomeDslExtensions
    {
        public static IServiceCollection AddPi4SmartHomeDslScanner(this IServiceCollection services)
        {
            services.AddTransient<IPi4SmartHomeDslScanner, Pi4SmartHomeDslScanner>();

            return services;
        }

        public static IServiceCollection AddPi4SmartHomeDslParser(this IServiceCollection services)
        {
            services.AddTransient<IPi4SmartHomeDslParser, Pi4SmartHomeDslParser>();

            return services;
        }
    }
}
