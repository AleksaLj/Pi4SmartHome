using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementDSL.Test
{
    internal class RabbitMQConsumerTest
    {
        public IMessageConsumer MessageConsumer { get; set; }

        public RabbitMQConsumerTest(IMessageConsumer messageConsumer)
        {
            MessageConsumer = messageConsumer;
        }

        public async void ReceiveMessage()
        {
            if (!MessageConsumer.IsConnected)
            {
                await MessageConsumer.ConnectAsync();
            }

            await MessageConsumer.OnMessage();
        }
    }
}
