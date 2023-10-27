using System.Fabric;
using System.Reflection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Extensions;

namespace AdminManagementWebApiService
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance.
    /// </summary>
    internal sealed class AdminManagementWebApiService : StatelessService
    {
        public AdminManagementWebApiService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        var builder = WebApplication.CreateBuilder();

                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);
                        builder.WebHost
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url);
                        
                        // Add services to the container.
                        
                        builder.Services.AddControllers();
                        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen();

                        //add services
                        builder.Services.AddRabbitMQConfiguration(builder.Configuration);

                        builder.Services.AddMessageProducer<AdminDSLMessage>(getExchangeName: () => builder.Configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLExchangeName").Value!,
                            getExchangeQueueRoutingKey: () => builder.Configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueRoutingKey").Value!,
                            getQueueName: () => builder.Configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueName").Value!);

                        //TO DO : Uncomment if once this is needed for Web API project!
                        //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                        var app = builder.Build();
                        
                        // Configure the HTTP request pipeline.
                        if (app.Environment.IsDevelopment())
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI();
                        }

                        app.UseHttpsRedirection();
                        
                        //app.UseAuthorization();
                        
                        app.MapControllers();
                        
                        return app;


                    }))
            };
        }
    }
}
