using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pi4SmartHome.Core.RabbitMQ.Configurations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Pi4SmartHome.Core.RabbitMQ.Implementations
{
    public class MessageConsumer : RabbitMQBase, IMessageConsumer
    {
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(IOptions<RabbitMQConfiguration> options, 
                               IServiceProvider services, 
                               ILogger<MessageConsumer> logger) : base(options, services)
        {
            _logger = logger;
        }

        public async Task OnMessage()
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);

                consumer.Received += async (model, eventArgs) =>
                {
                    var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    var message = JsonConvert.DeserializeObject(body);

                    Console.WriteLine("On message event...");

                    Channel?.BasicAck(eventArgs.DeliveryTag, false);

                    await Task.Yield();
                };


                string consumerTag = Channel.BasicConsume(RabbitMQConfig.AdminManagementDSLQueueName, false, consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while consuming message from queue.");
                throw;
            }
        }
    }
}
