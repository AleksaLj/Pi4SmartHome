using Microsoft.Extensions.DependencyInjection;
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
        private readonly ILogger<MessageProducer<TMessage>> _logger;
        private readonly object obj = new();
        private readonly string _exchange;
        private readonly string _exchangeQueueRoutingKey;

        public MessageProducer(IOptions<RabbitMQConfiguration> options, 
                               IServiceProvider services, 
                               ILogger<MessageProducer<TMessage>> logger,
                               string exchange,
                               string exchangeQueueRoutingKey) : base(options, services)
        {
            _logger = logger;
            _exchange = exchange;
            _exchangeQueueRoutingKey = exchangeQueueRoutingKey;
        }

        public async Task SendMessageAsync(TMessage message)
        {
            try
            {
                //var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var body = message.GetMessageData();
                var props = Channel?.CreateBasicProperties();
                props!.ContentType = "application/json";
                props!.DeliveryMode = 1;

                var exchange = _exchange;
                var routingKey = _exchangeQueueRoutingKey;

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
                _logger.LogError(ex, $"Error with publishing message.");
                throw;
            }
        }
    }
}
