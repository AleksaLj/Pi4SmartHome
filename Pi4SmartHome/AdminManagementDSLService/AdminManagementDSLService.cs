using System.Fabric;
using AdminManagementDSL.Service.Extensions;
using AdminManagementDSL.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace AdminManagementDSLService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class AdminManagementDSLService : StatelessService
    {
        private IHost _serviceHost = null!;

        private IServiceProvider Services => _serviceHost.Services;
        private IAdminManagementDSLService _adminManagementDSLService = null!;
        private ILogger? _log;

        public AdminManagementDSLService(StatelessServiceContext context)
            : base(context)
        { }

        private IAdminManagementDSLService GetAdminManagementDSLService()
        {
            if (_adminManagementDSLService == null)
            {
                _adminManagementDSLService = Services.GetAdminManagementDSLService();
            }

            return _adminManagementDSLService!;
        }

        private ILogger? Log 
        {
            get
            {
                if (_log == null)
                {
                    _log = Services?.GetService<ILogger<AdminManagementDSLService>>();
                }

                return _log;
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {





            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await GetAdminManagementDSLService().RunAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.Message($"Error:{ex.Message}");
                    Log?.LogError(ex, $"Error in {nameof(AdminManagementDSLService)}:{ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }
}
