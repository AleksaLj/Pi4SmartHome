using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using RabbitMQ.Client.Events;
using System.Threading;
using MediatR;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdminManagementDSL.Test
{
    internal class RabbitMQConsumerTest
    {
        public IMessageConsumer<AdminDSLMessage> MessageConsumer { get; set; }
        public IMessageProducer<AdminDSLInterpreterEndMessage> MessageProducer { get; set; }
        private readonly IMediator _mediator;
        private readonly IAdminDSLParser _parser;
        private readonly IAdminDSLInterpreter _interpreter;
        private readonly ILogger<AdminManagementDSLMsgHandler> _logger;

        public RabbitMQConsumerTest(IMessageConsumer<AdminDSLMessage> messageConsumer,
                                    IMediator mediator,
                                    IAdminDSLParser parser,
                                    IAdminDSLInterpreter interpreter,
                                    ILogger<AdminManagementDSLMsgHandler> logger,
                                    IMessageProducer<AdminDSLInterpreterEndMessage> messageProducer)
        {
            MessageConsumer = messageConsumer;
            _mediator = mediator;
            _parser = parser;
            _interpreter = interpreter;
            _logger = logger;
            MessageProducer = messageProducer;
        }

        public async Task ReceiveMessage()
        {
            MessageConsumer.AddMessageEventHandler(new AdminManagementDSLMsgHandler(_mediator, _parser, _interpreter, _logger));

            while (true) 
            {
                try
                {
                    if (!MessageConsumer.IsConnected)
                    {
                        await MessageConsumer.ConnectAsync();
                    }
                    MessageConsumer.OnMessageReceivedEvent += OnMessage;

                    await MessageConsumer.Subscribe();
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }                
            }            
        }

        private async Task OnMessage(object? sender, AdminDSLMessage message)
        {
            //generate message for end of the inserting process;
            var adminDslGuid = message.AdminDslGuid;

            var adminDslInterpreterEndMsg = new AdminDSLInterpreterEndMessage(adminDslGuid);

            if (!MessageProducer.IsConnected)
            {
                await MessageProducer.ConnectAsync();
            }

            await MessageProducer.SendMessageAsync(adminDslInterpreterEndMsg);
        }
    }
}
