
namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IMessageConsumer : IRabbitMQ
    {
        Task OnMessage();
    }
}
