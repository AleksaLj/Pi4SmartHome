using System.Fabric;
using CloudToDevice.Service.Extensions;
using CloudToDevice.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Pi4SmartHome.Core.RabbitMQ.Common.Extensions;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHomeDSL.DSL.Common.Extensions;
using Pi4SmartHomeDSL.Infrastructure.Common.Extensions;

namespace CloudToDeviceService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CloudToDeviceService : StatelessService
    {
        private IHost _serviceHost = null!;
        private IServiceProvider Services => _serviceHost.Services;
        private ILogger? _log;
        private ICloudToDeviceService? _cloudToDeviceService;

        public CloudToDeviceService(StatelessServiceContext context)
            : base(context)
        { }

        private ILogger? Log => _log ??= Services.GetService<ILogger<CloudToDeviceService>>();

        private ICloudToDeviceService GetCloudToDeviceService()
        {
            _cloudToDeviceService = Services.GetCloudToDeviceService();

            return _cloudToDeviceService;
        }

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

                    services.AddIoTHubConnection(configuration);
                    services.AddRabbitMQConfiguration(configuration);

                    services.AddPi4SmartHomeDslScanner();
                    services.AddPi4SmartHomeDslParser();
                    services.AddPi4SmartHomeDslInterpreter();
                    services.AddNodeVisitor();

                    services.AddMessageConsumer<CloudToDeviceMessage>(
                        getExchangeName: () => configuration.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslExchangeName").Value!,
                        getExchangeQueueRoutingKey: () =>
                            configuration.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslQueueRoutingKey").Value!,
                        getQueueName: () => configuration.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslQueueName").Value!);

                    services.AddCloudToDeviceMessageHandler();
                    services.AddCloudToDeviceService();
                    services.AddDeviceMessagingService();
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
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await GetCloudToDeviceService().RunAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.Message($"Error:{ex.Message}");
                    Log?.LogError(ex, $"Error in {nameof(CloudToDeviceService)}:{ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }
}
