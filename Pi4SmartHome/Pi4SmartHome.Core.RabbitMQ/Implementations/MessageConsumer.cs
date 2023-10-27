using Microsoft.Extensions.DependencyInjection;
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
        private readonly object _syncHandlerObj = new();

        private readonly List<IMessageEventHandlerService<TMessage>> _handlers;

        public MessageConsumer(IOptions<RabbitMQConfiguration> options,
                               IServiceProvider services, 
                               string exchange,
                               string exchangeQueueRoutingKey,
                               string queueName) : base(options, services)
        {
            Queue = queueName;
            Exchange = exchange;
            ExchangeQueueRoutingKey = exchangeQueueRoutingKey;
            _handlers = new List<IMessageEventHandlerService<TMessage>>();
            _handlers.AddRange(services.GetServices<IMessageEventHandlerService<TMessage>>());                        
        }

        public event AsyncEventHandler<TMessage>? OnMessageReceivedEvent;
        public void AddMessageEventHandler(IMessageEventHandlerService<TMessage> handler)
        {
            lock (_syncHandlerObj)
            {
                _handlers.Add(handler);
            }
        }

        public bool RemoveMessageEventHandler(IMessageEventHandlerService<TMessage> handler)
        {
            lock (_syncHandlerObj)
            {
                return _handlers.Remove(handler);
            }
        }

        public async Task Subscribe()
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);

                consumer.Received += async (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = QueueMessage.GetQueueMessage<TMessage>(body);
                    Log.LogInformation("{message}",message?.MessageId.ToString());

                    if (message != null) await OnMessage(message);

                    Channel?.BasicAck(eventArgs.DeliveryTag, true); //TO DO : REVISIT THIS PARAMETER true/false

                    await Task.Yield();
                };

                var consumerTag = Channel?.BasicConsume(Queue, true, consumer); //TO DO : REVISIT THIS PARAMETER true/false

                await TaskCache.True;
            }
            catch (Exception ex)
            {
                Log.LogError(ex, $"Error while consuming message from queue.");
                throw;
            }

            await TaskCache.True;
        }

        private async Task OnMessage(TMessage message)
        {
            IMessageEventHandlerService<TMessage>[]? all;
            lock (_syncHandlerObj) 
            {
                all = _handlers.ToArray();
            }

            foreach (var handler in all)
            {
                try
                {
                    await handler.OnMessage(message, this);
                }
                catch (Exception ex)
                {
                    Log.LogError(ex, "OnMessage error. {errMsg}", ex.Message);
                }
            }

            try
            {
                OnMessageReceivedEvent?.Invoke(this, message);
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "OnMessage error. {errMsg}", ex.Message);
            }
        }
    }
}
