using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client.Events;
using System.Threading;
using MediatR;

namespace AdminManagementDSL.Test
{
    internal class RabbitMQConsumerTest
    {
        public IMessageConsumer<AdminDSLMessage> MessageConsumer { get; set; }
        private readonly IMediator _mediator;

        public RabbitMQConsumerTest(IMessageConsumer<AdminDSLMessage> messageConsumer,
                                    IMediator mediator)
        {
            MessageConsumer = messageConsumer;
            _mediator = mediator;
        }

        public async Task ReceiveMessage()
        {          
            while (true) 
            {
                try
                {
                    if (!MessageConsumer.IsConnected)
                    {
                        await MessageConsumer.ConnectAsync();
                    }
                    MessageConsumer.AddMessageEventHandler(new AdminManagementDSLMsgHandler(_mediator));
                    MessageConsumer.OnMessageReceivedEvent += OnMessage;

                    await MessageConsumer.Subscribe();
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }                
            }            
        }

        private static void OnMessage(object? sender, AdminDSLMessage message)
        {
            Console.WriteLine("OnMessage in Consumer");
            Console.WriteLine($"Source code: {message.DslSourceCode}");

        }
    }
}
