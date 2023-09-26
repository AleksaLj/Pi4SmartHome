using Pi4SmartHome.Core.RabbitMQ.Common.Messages;

namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IMessageConsumer<TMessage> : IRabbitMQ where TMessage : QueueMessage
    {
        bool RemoveMessageEventHandler(IMessageEventHandlerService<TMessage> handler);
        void AddMessageEventHandler(IMessageEventHandlerService<TMessage> handler);
        event EventHandler<TMessage>? OnMessageReceivedEvent;
        Task Subscribe();
    }
}
