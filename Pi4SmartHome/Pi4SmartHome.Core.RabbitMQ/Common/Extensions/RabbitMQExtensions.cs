using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
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

        public static IServiceCollection AddMessageProducer<TMessage>(this IServiceCollection services, 
                                                                           Func<string> getExchangeName,
                                                                           Func<string> getExchangeQueueRoutingKey,
                                                                           Func<string> getQueueName) where TMessage : QueueMessage
        {
            services.AddSingleton(typeof(IMessageProducer<TMessage>), (srv) =>
            {
                return new MessageProducer<TMessage>(srv.GetService<IOptions<RabbitMQConfiguration>>()!,
                                                     srv,
                                                     exchange: getExchangeName(),
                                                     exchangeQueueRoutingKey: getExchangeQueueRoutingKey(),
                                                     queue: getQueueName());
            });

            return services;
        }

        public static IServiceCollection AddMessageConsumer<TMessage>(this IServiceCollection services,
                                                                      Func<string> getExchangeName,
                                                                      Func<string> getExchangeQueueRoutingKey,
                                                                      Func<string> getQueueName) where TMessage : QueueMessage
        {
            services.AddSingleton(typeof(IMessageConsumer<TMessage>), (srv) => 
            {
                return new MessageConsumer<TMessage>(srv.GetService<IOptions<RabbitMQConfiguration>>()!,
                                                     srv,
                                                     exchange: getExchangeName(),
                                                     exchangeQueueRoutingKey: getExchangeQueueRoutingKey(),
                                                     queueName: getQueueName());
            });

            return services;
        }

        public static RabbitMQConfiguration GetRabbitMQConfiguration(this IConfiguration config)
        {
            return config.GetSection(RABBITMQ_CONFIG_ROOT).Get<RabbitMQConfiguration>()!;
        }

        public static IMessageProducer<TMessage>? GetMessageProducer<TMessage>(this IServiceProvider services) where TMessage : QueueMessage
        {
            return services.GetService<IMessageProducer<TMessage>>();
        }

        public static IMessageConsumer<TMessage>? GetMessageConsumer<TMessage>(this IServiceProvider services) where TMessage : QueueMessage
        {
            return services.GetService<IMessageConsumer<TMessage>>();
        }
    }
}
