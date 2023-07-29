using Microsoft.Extensions.Logging;

namespace Pi4SmartHome.Core.Implementations
{
    public class ServiceFabricHostedServiceBase : ServiceBase
    {
        public ServiceFabricHostedServiceBase(IServiceProvider serviceProvider, ILogger log) : base(serviceProvider, log)
        {
        }

        public void Abort() => OnAbort();

        protected virtual void OnAbort()
        {
        }

        public async Task OpenAsync(CancellationToken cancellationToken) => await OnOpenAsync(cancellationToken);

        protected virtual async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task CloseAsync(CancellationToken cancellationToken) => await OnCloseAsync(cancellationToken);

        protected virtual async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task RunAsync(CancellationToken cancellationToken) => await OnRunAsync(cancellationToken);

        protected virtual async Task OnRunAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
