using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pi4SmartHomeDSL.Application.Interfaces;
using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.Core.Configurations;
using Microsoft.Azure.Devices;

namespace Pi4SmartHomeDSL.Infrastructure.CloudToDeviceService
{
    public class IoTHubCloudToDeviceMessagingService : IIoTHubCloudToDeviceMessagingService
    {
        private readonly ILogger<IoTHubCloudToDeviceMessagingService> _logger;
        private readonly IDisposable? _iotHubConnHandle;
        private IoTHubConnectionOptions _ioTHubConnection;
        private readonly ServiceClient _serviceClient;

        public IoTHubCloudToDeviceMessagingService(IOptionsMonitor<IoTHubConnectionOptions> ioTHubConnection, ILogger<IoTHubCloudToDeviceMessagingService> logger)
        {
            _logger = logger;
            _ioTHubConnection = ioTHubConnection.CurrentValue;
            _iotHubConnHandle = ioTHubConnection.OnChange(OnIoTHubConnectionChange);
            _serviceClient = ServiceClient.CreateFromConnectionString(_ioTHubConnection.IoTHubConnectionString);
        }

        private void OnIoTHubConnectionChange(IoTHubConnectionOptions ioTHubConnection)
        {
            _ioTHubConnection = ioTHubConnection;
        }

        public async Task SendMessageToDeviceAsync(IoTDeviceMessage message)
        {
            try
            {
                var messageBytes = message.GetIoTDeviceMessageData();

                await _serviceClient.SendAsync(message.DeviceId, new Message(messageBytes));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        ~IoTHubCloudToDeviceMessagingService()
        {
            _iotHubConnHandle?.Dispose();
        }
    }
}
