using CloudToDevice.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHomeDSL.Application.Interfaces;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;
using Pi4SmartHomeDSL.DSL.Scanner;

namespace CloudToDevice.Service.Implementations
{
    public class CloudToDeviceMessageHandler : ICloudToDeviceMessageHandler
    {
        private readonly IIoTHubCloudToDeviceMessagingService _iotCloudToDeviceService;
        private readonly IPi4SmartHomeDslInterpreter _interpreter;
        private readonly IPi4SmartHomeDslParser _parser;
        private readonly ILogger<CloudToDeviceMessageHandler> _logger;

        public CloudToDeviceMessageHandler(IPi4SmartHomeDslInterpreter interpreter,
                                            IPi4SmartHomeDslParser parser,
                                            ILogger<CloudToDeviceMessageHandler> logger,
                                            IIoTHubCloudToDeviceMessagingService iotCloudToDeviceService)
        {
            _interpreter = interpreter;
            _parser = parser;
            _logger = logger;
            _iotCloudToDeviceService = iotCloudToDeviceService;
        }

        public async Task OnMessage(CloudToDeviceMessage message, object sender)
        {
            if (string.IsNullOrEmpty(message.CloudToDeviceMessageSourceCode))
            {
                _logger.LogError("Parameter 'cloudToDeviceMessageSourceCode' is empty!");
                throw new ArgumentException("Parameter 'cloudToDeviceMessageSourceCode' is empty!");
            }

            var scanner = new Pi4SmartHomeDslScanner();
            await scanner.Configure(message.CloudToDeviceMessageSourceCode);
            var tree = await _parser.Parse(scanner);
            var iotDeviceMessageInterpreted = await _interpreter.Interpret(tree);

            await _iotCloudToDeviceService.SendMessageToDeviceAsync(iotDeviceMessageInterpreted);
        }
    }
}
