using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pi4SmartHome.Core.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Configurations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace Pi4SmartHome.Core.RabbitMQ.Implementations
{
    public class RabbitMQBase : ServiceBase, IRabbitMQ
    {
        protected IConnection? Connection { get; set; }
        protected IModel? Channel { get; set; }
        protected readonly RabbitMQConfiguration RabbitMQConfig;
        readonly object obj = new object();

        public RabbitMQBase(IOptions<RabbitMQConfiguration> options, IServiceProvider services) : base(services, services.GetService<ILogger<RabbitMQBase>>()!)
        {
            RabbitMQConfig = options.Value;
        }

        public bool IsConnected => Connection != null && Connection.IsOpen == true;

        public async Task ConnectAsync()
        {
            if (!IsConnected)
            {
                lock (obj)
                {
                    try
                    {
                        var factory = new ConnectionFactory
                        {
                            HostName = RabbitMQConfig.Host,
                            UserName = RabbitMQConfig.Username,
                            Password = RabbitMQConfig.Password,
                            VirtualHost = RabbitMQConfig.VirtualHost,
                            AutomaticRecoveryEnabled = true,
                            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                            DispatchConsumersAsync = true
                        };

                        if (Connection == null || Connection.IsOpen == false)
                        {
                            Connection = factory.CreateConnection();
                        }

                        if (Channel == null || Channel.IsOpen == false)
                        {
                            Channel = Connection?.CreateModel();
                            //Services.AdminManagementDSL
                            Channel.ExchangeDeclare(exchange: RabbitMQConfig.AdminManagementDSLExchangeName, ExchangeType.Direct);
                            Channel?.QueueDeclare(queue: RabbitMQConfig.AdminManagementDSLQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                            Channel.QueueBind(queue: RabbitMQConfig.AdminManagementDSLQueueName, exchange: RabbitMQConfig.AdminManagementDSLExchangeName, routingKey: RabbitMQConfig.AdminManagementDSLQueueRoutingKey);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogError(ex, $"Unsuccess connection to {RabbitMQConfig.AdminManagementDSLExchangeName} exchange, {RabbitMQConfig.AdminManagementDSLQueueName}.");
                        Channel = null;
                        Connection = null;
                    }
                }
            }            
        }

        public async Task DisconnectAsync()
        {
            if (IsConnected)
            {
                lock (obj)
                {
                    if (Connection != null)
                    {
                        Connection.Close();
                        Connection = null;
                    }
                }                
            }
        }
    }
}
