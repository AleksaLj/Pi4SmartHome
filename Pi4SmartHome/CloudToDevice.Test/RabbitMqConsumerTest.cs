using CloudToDevice.Service.Implementations;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using Pi4SmartHomeDSL.Application.Interfaces;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace CloudToDevice.Test
{
    public class RabbitMqConsumerTest
    {
        private IMessageConsumer<CloudToDeviceMessage> MessageConsumer { get; set; }
        private readonly IIoTHubCloudToDeviceMessagingService _iotCloudToDeviceService;
        private readonly IPi4SmartHomeDslInterpreter _interpreter;
        private readonly IPi4SmartHomeDslParser _parser;
        private readonly ILogger<CloudToDeviceMessageHandler> _logger;

        public RabbitMqConsumerTest(IMessageConsumer<CloudToDeviceMessage> messageConsumer,
            IIoTHubCloudToDeviceMessagingService iotCloudToDeviceService,
            IPi4SmartHomeDslInterpreter interpreter,
            IPi4SmartHomeDslParser parser,
            ILogger<CloudToDeviceMessageHandler> logger)
        {
            MessageConsumer = messageConsumer;
            _iotCloudToDeviceService = iotCloudToDeviceService;
            _interpreter = interpreter;
            _parser = parser;
            _logger = logger;
        }

        public async Task ReceiveMessage()
        {
            var messageHandler =
                new CloudToDeviceMessageHandler(_interpreter, _parser, _logger, _iotCloudToDeviceService);
            MessageConsumer.AddMessageEventHandler(messageHandler);

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
                    _logger.LogError("Message error: {x01}", ex.Message);
                }
            }
        }

        private async Task OnMessage(object? sender, CloudToDeviceMessage message)
        {
            _logger.LogInformation("Finished interpretation of message {x01}, pi4SmartHomeDsl code: {x02}", nameof(message), message.CloudToDeviceMessageSourceCode);

            await TaskCache.True;
        }
    }
}
