using DeviceManagement.Application.Interfaces;
using DeviceManagement.Core.Configurations;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeviceManagement.Infrastructure.IoTHubDeviceService
{
    public class IoTHubDeviceProvisioningService : IIoTHubDeviceProvisioningService
    {
        private readonly IDisposable? _iotHubConnHandle;
        private IoTHubConnectionOptions _ioTHubConnection;
        private readonly RegistryManager _registryManager;
        private ILogger<IoTHubDeviceProvisioningService> _logger;

        public IoTHubDeviceProvisioningService(IOptionsMonitor<IoTHubConnectionOptions> ioTHubConnection, ILogger<IoTHubDeviceProvisioningService> logger)
        {
            _ioTHubConnection = ioTHubConnection.CurrentValue;
            _iotHubConnHandle = ioTHubConnection.OnChange(OnIoTHubConnectionChange);
            _registryManager = RegistryManager.CreateFromConnectionString(_ioTHubConnection.IoTHubConnectionString);
            _logger = logger;
        }

        private void OnIoTHubConnectionChange(IoTHubConnectionOptions ioTHubConnection)
        {
            _ioTHubConnection = ioTHubConnection;
        }

        public async Task<IEnumerable<Device>> AddDevicesToIoTHubAsync(IEnumerable<string> deviceIds)
        {
            var items = new List<Device>();

            foreach (var deviceId in deviceIds)
            {
                var addedDevice = await AddDeviceAsync(deviceId);

                items.Add(addedDevice);
            }

            return items;
        }

        private async Task<Device> AddDeviceAsync(string deviceId)
        {
            Device device;

            try
            {
                var newDevice = new Device(deviceId);

                device =  await _registryManager.AddDeviceAsync(newDevice);

            }
            catch (DeviceAlreadyExistsException)
            {
                device = await _registryManager.GetDeviceAsync(deviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            return device;
        }

        ~IoTHubDeviceProvisioningService()
        {
            _iotHubConnHandle?.Dispose();
        }
    }
}
