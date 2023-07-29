using AdminManagementDSL.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.Implementations;

namespace AdminManagementDSL.Service.Implementations
{
    public class AdminManagementDSLService : ServiceFabricHostedServiceBase, IAdminManagementDSLService
    {
        public AdminManagementDSLService(IServiceProvider serviceProvider, 
                                         ILogger<AdminManagementDSLService> log) : base(serviceProvider, log)
        {

        }

        protected override void OnAbort()
        {
            Log.LogInformation($"Execute {nameof(AdminManagementDSLService)}.{nameof(OnAbort)}.");
        }

        protected override async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(AdminManagementDSLService)}.{nameof(OnOpenAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(AdminManagementDSLService)}.{nameof(OnCloseAsync)}.");
            await TaskCache.True;
        }

        protected override Task OnRunAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Started execution {nameof(AdminManagementDSLService)}.{nameof(OnRunAsync)}.");


            //implement logic;


            Log.LogInformation($"Finished execution {nameof(AdminManagementDSLService)}.{nameof(OnRunAsync)}.");

            return base.OnRunAsync(cancellationToken);
        }
    }
}
