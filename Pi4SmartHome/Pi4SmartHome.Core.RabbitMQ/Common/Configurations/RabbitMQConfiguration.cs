
namespace Pi4SmartHome.Core.RabbitMQ.Configurations
{
    public class RabbitMQConfiguration
    {
        public string Host { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string VirtualHost { get; set; } = null!;
        public string AdminManagementDSLExchangeName { get; set; } = null!;
        public string AdminManagementDSLQueueName { get; set; } = null!;
        public string AdminManagementDSLQueueRoutingKey { get; set; } = null!;
    }
}
