using Microsoft.Extensions.DependencyInjection;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;
using Pi4SmartHomeDSL.DSL.Interpreter;
using Pi4SmartHomeDSL.DSL.Interpreter.Visitor;
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

        public static IServiceCollection AddPi4SmartHomeDslInterpreter(this IServiceCollection services)
        {
            services.AddTransient<IPi4SmartHomeDslInterpreter, Pi4SmartHomeDslInterpreter>();

            return services;
        }

        public static IServiceCollection AddNodeVisitor(this IServiceCollection services)
        {
            services.AddTransient<INodeVisitor, NodeVisitor>();

            return services;
        }

        public static IPi4SmartHomeDslParser GetPi4SmartHomeDslParser(this IServiceProvider services)
        {
            return services.GetService<IPi4SmartHomeDslParser>()!;
        }

        public static IPi4SmartHomeDslInterpreter GetPi4SmartHomeDslInterpreter(this IServiceProvider services)
        {
            return services.GetService<IPi4SmartHomeDslInterpreter>()!;
        }
    }
}
