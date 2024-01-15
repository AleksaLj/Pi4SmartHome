using System.Fabric;
using AdminManagementDSL.AdminDSL.Common.Extensions;
using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Infrastructure.Common.Extensions;
using AdminManagementDSL.Service.Extensions;
using AdminManagementDSL.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Common.Extensions;

namespace AdminManagementDSLService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class AdminManagementDSLService : StatelessService
    {
        private IHost _serviceHost = null!;

        private IServiceProvider Services => _serviceHost.Services;
        private IAdminManagementDSLService? _adminManagementDslService;
        private ILogger? _log;

        public AdminManagementDSLService(StatelessServiceContext context)
            : base(context)
        { }

        private IAdminManagementDSLService GetAdminManagementDSLService()
        {
            _adminManagementDslService = Services.GetAdminManagementDSLService();

            return _adminManagementDslService;
        }

        private ILogger? Log => _log ??= Services.GetService<ILogger<AdminManagementDSLService>>();

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
                    //RabbitMQ Services
                    services.AddRabbitMQConfiguration(configuration);
                    services.AddMessageConsumer<AdminDSLMessage>(getExchangeName: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLExchangeName").Value!,
                        getExchangeQueueRoutingKey: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueRoutingKey").Value!,
                        getQueueName: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueName").Value!);

                    services.AddMessageProducer<AdminDSLInterpreterEndMessage>(getExchangeName: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndExchangeName").Value!,
                        getExchangeQueueRoutingKey: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueRoutingKey").Value!,
                        getQueueName: () => configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueName").Value!);

                    //AdminDSL Services
                    services.AddSqlConnOptions(configuration);
                    services.AddAdminDSLParser();
                    services.AddNodeVisitor();
                    services.AddAdminDSLInterpreter();
                    services.AddAdminDslRepo();
                    services.AddAdminManagementDSLService();
                    services.AdminManagementDslMsgHandler();

                    //MediatR
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateAdminDSLCommand>());
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
