
namespace Pi4SmartHome.Core.Interfaces
{
    public interface IServiceFabricHostedService : IPi4SmartHomeService
    {
        void Abort();
        Task OpenAsync(CancellationToken cancellationToken);
        Task CloseAsync(CancellationToken cancellationToken);
        Task RunAsync(CancellationToken cancellationToken);
    }
}
