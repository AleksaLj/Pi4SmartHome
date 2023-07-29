
namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IMessageProducer : IRabbitMQ
    {
        public Task SendMessageAsync<TMessage>(TMessage message);
    }
}
