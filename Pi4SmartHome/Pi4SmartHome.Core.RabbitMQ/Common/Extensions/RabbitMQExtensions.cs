
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pi4SmartHome.Core.RabbitMQ.Configurations;
using Pi4SmartHome.Core.RabbitMQ.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace Pi4SmartHome.Core.RabbitMQ.Extensions
{
    public static class RabbitMQExtensions
    {
        private const string RABBITMQ_CONFIG_ROOT = $"rabbitMQ:Configuration";

        public static IServiceCollection AddRabbitMQConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<RabbitMQConfiguration>().Bind(config.GetSection(RABBITMQ_CONFIG_ROOT));

            return services;
        }

        public static IServiceCollection AddMessageProducer(this IServiceCollection services, IConfiguration? config = null)
        {
            services.AddSingleton<IMessageProducer, MessageProducer>();

            return services;
        }

        public static IServiceCollection AddMessageConsumer(this IServiceCollection services, IConfiguration? config = null)
        {
            services.AddSingleton<IMessageConsumer, MessageConsumer>();

            return services;
        }

        public static RabbitMQConfiguration GetRabbitMQConfiguration(this IConfiguration config)
        {
            return config.GetSection(RABBITMQ_CONFIG_ROOT).Get<RabbitMQConfiguration>()!;
        }

        public static IMessageProducer? GetMessageProducer(this IServiceProvider services)
        {
            return services.GetService<IMessageProducer>();
        }

        public static IMessageConsumer? GetMessageConsumer(this IServiceProvider services)
        {
            return services.GetService<IMessageConsumer>();
        }
    }
}
