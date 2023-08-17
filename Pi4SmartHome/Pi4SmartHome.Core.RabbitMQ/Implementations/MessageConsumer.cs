using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Configurations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pi4SmartHome.Core.RabbitMQ.Implementations
{
    public class MessageConsumer<TMessage> : RabbitMQBase, IMessageConsumer<TMessage> where TMessage : QueueMessage
    {
        private readonly ILogger<MessageConsumer<TMessage>> _logger;
        private readonly string _queueName;

        public MessageConsumer(IOptions<RabbitMQConfiguration> options, 
                               IServiceProvider services, 
                               ILogger<MessageConsumer<TMessage>> logger,
                               string queueName) : base(options, services)
        {
            _logger = logger;
            _queueName = queueName;
        }

        public async Task OnMessage()
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);

                consumer.Received += async (model, eventArgs) =>
                {
                    //var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    //var message = JsonConvert.DeserializeObject(body);

                    var body = eventArgs.Body.ToArray();
                    var message = QueueMessage.GetQueueMessage<TMessage>(body);
                    _logger.LogInformation(message.MessageId.ToString());
                    Console.WriteLine(body);
                    Console.WriteLine();

                    Console.WriteLine("On message event...");

                    Channel?.BasicAck(eventArgs.DeliveryTag, false);

                    await Task.Yield();
                };

                string consumerTag = Channel.BasicConsume(_queueName, false, consumer);

                await TaskCache.True;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while consuming message from queue.");
                throw;
            }
        }
    }
}
