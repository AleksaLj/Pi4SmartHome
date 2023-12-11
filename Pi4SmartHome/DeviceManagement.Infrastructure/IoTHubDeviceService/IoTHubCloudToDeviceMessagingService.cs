using DeviceManagement.Application.Interfaces;
using DeviceManagement.Application.Models;
using DeviceManagement.Core.Configurations;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeviceManagement.Infrastructure.IoTHubDeviceService
{
    public class IoTHubCloudToDeviceMessagingService : IIoTHubCloudToDeviceMessagingService
    {
        private readonly IDisposable? _iotHubConnHandle;
        private IoTHubConnectionOptions _ioTHubConnection;
        private readonly ServiceClient _serviceClient;

        public IoTHubCloudToDeviceMessagingService(IOptionsMonitor<IoTHubConnectionOptions> ioTHubConnection, ILogger<IoTHubDeviceProvisioningService> logger)
        {
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
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
