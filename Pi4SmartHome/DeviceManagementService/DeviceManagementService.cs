using System.Fabric;
using DeviceManagement.Infrastructure.Common.Extensions;
using DeviceManagement.Service.Extensions;
using DeviceManagement.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Common.Extensions;

namespace DeviceManagementService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class DeviceManagementService : StatelessService
    {
        private IHost _serviceHost = null!;
        private IDeviceManagementService? _deviceManagementService;
        private ILogger? _log;

        private IServiceProvider Services => _serviceHost.Services;

        private IDeviceManagementService GetDeviceManagementService()
        {
            _deviceManagementService ??= Services.GetDeviceManagementService();

            return _deviceManagementService;
        }

        public DeviceManagementService(StatelessServiceContext context)
            : base(context)
        { }

        private ILogger? Log => _log ??= Services.GetService<ILogger<DeviceManagementService>>();

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var environment = FabricRuntime.GetActivationContext()?
                .GetConfigurationPackageObject("Config")?
                .Settings.Sections["Environment"]?
                .Parameters["ASPNETCORE_ENVIRONMENT"]?.Value;

            _serviceHost = new HostBuilder()
               .UseEnvironment(environment)
               .ConfigureAppConfiguration((_, config) =>
               {
                   config.SetBasePath(
                       Context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Path);
                   config.AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);
               })
               .ConfigureLogging((hostContext, configLogging) =>
               {
                   configLogging.AddDebug();
               })
               .ConfigureServices((hostContext, services) =>
               {
                   var configuration = hostContext.Configuration;

                   services.AddSingleton(configuration);

                   services.AddSqlConnOptions(configuration);
                   services.AddIoTHubConnection(configuration);

                   services.AddMessageConsumer<AdminDSLInterpreterEndMessage>(getExchangeName: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndExchangeName").Value!,
                       getExchangeQueueRoutingKey: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueRoutingKey").Value!,
                       getQueueName: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueName").Value!);

                   services.AddIoTDeviceRepo();

                   services.AddDeviceManagementService();
                   services.AddAdminDslInterpreterEndMsgHandler();
                   services.AddDeviceProvisioningService();
               })
               .Build();

            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await GetDeviceManagementService().RunAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.Message($"Error:{ex.Message}");
                    Log?.LogError(ex, $"Error in {nameof(DeviceManagementService)}:{ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }
}
