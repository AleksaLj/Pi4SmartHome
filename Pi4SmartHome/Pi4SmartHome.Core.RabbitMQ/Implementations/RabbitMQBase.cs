using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Configurations;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace Pi4SmartHome.Core.RabbitMQ.Implementations
{
    public class RabbitMQBase : ServiceBase, IRabbitMQ
    {
        readonly object obj = new();

        protected IConnection? Connection { get; set; }
        protected IModel? Channel { get; set; }
        protected readonly RabbitMQConfiguration RabbitMQConfig;
        protected string? Queue { get; set; }
        protected string? Exchange { get; set; }
        protected string? ExchangeQueueRoutingKey { get; set; }

        public RabbitMQBase(IOptions<RabbitMQConfiguration> options, IServiceProvider services) : base(services, services.GetService<ILogger<RabbitMQBase>>()!)
        {
            RabbitMQConfig = options.Value;
        }

        public bool IsConnected
        {
            get 
            {
                lock (obj)
                {
                    return Connection is { IsOpen: true };
                }
            }
        }

        public async Task<bool> ConnectAsync()
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
                            Channel.ExchangeDeclare(exchange: Exchange, ExchangeType.Direct);
                            Channel?.QueueDeclare(queue: Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                            Channel.QueueBind(queue: Queue, exchange: Exchange, routingKey: ExchangeQueueRoutingKey);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogError(ex, "Unsuccessful connection to {Exchange} exchange, {Queue}.", Exchange, Queue);
                        Channel = null;
                        Connection = null;
                    }
                }
            }

            return await TaskCache.True;
        }

        public async Task<bool> DisconnectAsync()
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

            return await TaskCache.True;
        }
    }
}
