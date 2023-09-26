using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Configurations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace Pi4SmartHome.Core.RabbitMQ.Implementations
{
    public class MessageProducer<TMessage> : RabbitMQBase, IMessageProducer<TMessage> where TMessage : QueueMessage
    {
        private readonly object obj = new();

        public MessageProducer(IOptions<RabbitMQConfiguration> options,
                               IServiceProvider services, 
                               string exchange,
                               string exchangeQueueRoutingKey,
                               string queue) : base(options, services)
        {
            Exchange = exchange;
            ExchangeQueueRoutingKey = exchangeQueueRoutingKey;
            Queue = queue;
        }

        public async Task SendMessageAsync(TMessage message)
        {
            try
            {
                var body = message.GetMessageData();
                var props = Channel?.CreateBasicProperties();
                props!.ContentType = "application/json";
                props!.DeliveryMode = 1;

                var exchange = Exchange;
                var routingKey = ExchangeQueueRoutingKey;

                lock (obj)
                {
                    Channel?.BasicPublish(exchange: exchange,
                                          routingKey: routingKey,
                                          basicProperties: props,
                                          body: body);
                }

                await TaskCache.True;
            }
            catch (Exception ex)
            {
                Log.LogError(ex, $"Error with publishing message.");
                throw;
            }
        }
    }
}
