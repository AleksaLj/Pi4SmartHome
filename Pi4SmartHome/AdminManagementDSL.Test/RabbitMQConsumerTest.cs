using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementDSL.Test
{
    internal class RabbitMQConsumerTest
    {
        public IMessageConsumer<AdminDSLMessage> MessageConsumer { get; set; }

        public RabbitMQConsumerTest(IMessageConsumer<AdminDSLMessage> messageConsumer)
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
