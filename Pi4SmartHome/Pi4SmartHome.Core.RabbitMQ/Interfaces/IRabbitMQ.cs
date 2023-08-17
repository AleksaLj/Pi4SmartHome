
namespace Pi4SmartHome.Core.RabbitMQ.Interfaces
{
    public interface IRabbitMQ
    {
        Task<bool> ConnectAsync();
        Task<bool> DisconnectAsync();
        bool IsConnected { get; }
    }
}
