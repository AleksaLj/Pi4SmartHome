using CloudToDevice.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.Implementations;

namespace CloudToDevice.Service.Implementations
{
    public class CloudToDeviceService : ServiceFabricHostedServiceBase, ICloudToDeviceService
    {
        public CloudToDeviceService(IServiceProvider serviceProvider, ILogger log) : base(serviceProvider, log)
        {
        }

        protected override void OnAbort()
        {
            Log.LogInformation($"Execute {nameof(CloudToDeviceService)}.{nameof(OnAbort)}.");
        }

        protected override async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(CloudToDeviceService)}.{nameof(OnOpenAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(CloudToDeviceService)}.{nameof(OnCloseAsync)}.");
            await TaskCache.True;
        }

        protected override Task OnRunAsync(CancellationToken cancellationToken)
        {
            return base.OnRunAsync(cancellationToken);
        }
    }
}
