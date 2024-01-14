using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using MediatR;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using Microsoft.Extensions.Logging;
using AdminManagementDSL.Application.Device.Queries;

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
                    _logger.LogError(ex.Message);
                }                
            }            
        }

        private async Task OnMessage(object? sender, AdminDSLMessage message)
        {
            var adminDslGuid = message.AdminDslGuid;

            var getDevicesForIoTHubQuery = new GetDevicesForIoTHubQuery(adminDslGuid.ToString());
            var addIoTDeviceModels = await _mediator.Send(getDevicesForIoTHubQuery);

            var adminDslInterpreterEndMsg = new AdminDSLInterpreterEndMessage(adminDslGuid, addIoTDeviceModels.Select(item => item.DeviceIoTHubId));

            if (!MessageProducer.IsConnected)
            {
                await MessageProducer.ConnectAsync();
            }

            await MessageProducer.SendMessageAsync(adminDslInterpreterEndMsg);
        }
    }
}
