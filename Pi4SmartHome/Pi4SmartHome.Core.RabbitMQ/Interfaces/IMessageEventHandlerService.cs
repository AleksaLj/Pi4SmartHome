using Pi4SmartHome.Core.RabbitMQ.Common.Messages;

namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IMessageEventHandlerService<TMessage> where TMessage : QueueMessage
    {
        Task OnMessage(TMessage message, object sender);
    }
}
