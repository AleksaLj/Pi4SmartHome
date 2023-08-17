using Pi4SmartHome.Core.RabbitMQ.Common.Messages;

namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IMessageConsumer<TMessage> : IRabbitMQ where TMessage : QueueMessage
    {
        Task OnMessage();
    }
}
