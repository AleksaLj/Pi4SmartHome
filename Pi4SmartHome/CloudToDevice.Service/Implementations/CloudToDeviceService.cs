using CloudToDevice.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.Implementations;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using Pi4SmartHomeDSL.Application.Interfaces;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace CloudToDevice.Service.Implementations
{
    public class CloudToDeviceService : ServiceFabricHostedServiceBase, ICloudToDeviceService
    {
        private readonly IMessageConsumer<CloudToDeviceMessage> _messageConsumer;
        private readonly IIoTHubCloudToDeviceMessagingService _iotCloudToDeviceService;
        private readonly IPi4SmartHomeDslInterpreter _interpreter;
        private readonly IPi4SmartHomeDslParser _parser;
        private readonly ICloudToDeviceMessageHandler _cloudToDeviceMessageHandler;

        public CloudToDeviceService(IServiceProvider serviceProvider, 
                                    ILogger log,
                                    IMessageConsumer<CloudToDeviceMessage> messageConsumer,
                                    IIoTHubCloudToDeviceMessagingService iotCloudToDeviceService,
                                    IPi4SmartHomeDslInterpreter interpreter,
                                    IPi4SmartHomeDslParser parser,
                                    ICloudToDeviceMessageHandler cloudToDeviceMessageHandler) : base(serviceProvider, log)
        {
            _messageConsumer = messageConsumer;
            _iotCloudToDeviceService = iotCloudToDeviceService;
            _interpreter = interpreter;
            _parser = parser;
            _cloudToDeviceMessageHandler = cloudToDeviceMessageHandler;
        }

        protected override void OnAbort()
        {
            Log.LogInformation($"Execute {nameof(CloudToDeviceService)}.{nameof(OnAbort)}.");
        }

        protected override async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(CloudToDeviceService)}.{nameof(OnOpenAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Execute {nameof(CloudToDeviceService)}.{nameof(OnCloseAsync)}.");
            await TaskCache.True;
        }

        protected override async Task OnRunAsync(CancellationToken cancellationToken)
        {
            Log.LogInformation($"Started execution {nameof(CloudToDeviceService)}.{nameof(OnRunAsync)}.");

            _messageConsumer.AddMessageEventHandler(_cloudToDeviceMessageHandler);

            try
            {
                if (!_messageConsumer.IsConnected)
                {
                    await _messageConsumer.ConnectAsync();
                }

                _messageConsumer.OnMessageReceivedEvent += OnMessage;

                await _messageConsumer.Subscribe();
            }
            catch (Exception ex)
            {
                Log.LogError(ex, "Cannot process message. Error message: {0x1}", ex.Message);
            }

            Log.LogInformation($"Finished execution {nameof(CloudToDeviceService)}.{nameof(OnRunAsync)}.");
        }

        private async Task OnMessage(object? sender, CloudToDeviceMessage message)
        {
            Log.LogInformation("Finished interpretation of message {x01}, pi4SmartHomeDsl code: {x02}", nameof(message), message.CloudToDeviceMessageSourceCode);

            await TaskCache.True;
        }
    }
}
