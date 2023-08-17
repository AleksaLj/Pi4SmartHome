using Pi4SmartHome.Core.RabbitMQ.Common.Messages;

namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IMessageProducer<TMessage> : IRabbitMQ where TMessage : QueueMessage
    {
        public Task SendMessageAsync(TMessage message);
    }
}
