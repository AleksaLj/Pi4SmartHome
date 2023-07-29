using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pi4SmartHome.Core.RabbitMQ.Configurations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace Pi4SmartHome.Core.RabbitMQ.Implementations
{
    public class MessageProducer : RabbitMQBase, IMessageProducer
    {
        private readonly ILogger<MessageProducer> _logger;
        readonly object obj = new object();

        public MessageProducer(IOptions<RabbitMQConfiguration> options, IServiceProvider services, ILogger<MessageProducer> logger) : base(options, services)
        {
            _logger = logger;
        }

        public async Task SendMessageAsync<TMessage>(TMessage message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var props = Channel?.CreateBasicProperties();
                props!.ContentType = "application/json";
                props!.DeliveryMode = 1;

                var exchange = RabbitMQConfig.AdminManagementDSLExchangeName;
                var routingKey = RabbitMQConfig.AdminManagementDSLQueueRoutingKey;

                lock (obj)
                {
                    Channel?.BasicPublish(exchange: exchange,
                                      routingKey: routingKey,
                                      basicProperties: props,
                                      body: body);
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error with publishing message.");
                throw;
            }
        }
    }
}
