using DeviceManagement.Application.Interfaces;
using DeviceManagement.Application.Mappers;
using DeviceManagement.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;

namespace DeviceManagement.Service.Implementations
{
    public class AdminDSLInterpreterEndMsgHandler : IAdminDSLInterpreterEndMsgHandler
    {
        private readonly ILogger<AdminDSLInterpreterEndMsgHandler> _logger;
        private readonly IIoTHubDeviceProvisioningService _iotHubProvisioningService;
        private readonly IIoTDeviceRepo _ioTDeviceRepo;

        public AdminDSLInterpreterEndMsgHandler(ILogger<AdminDSLInterpreterEndMsgHandler> logger,
                                                IIoTHubDeviceProvisioningService iotHubProvisioningService,
                                                IIoTDeviceRepo ioTDeviceRepo)
        {
            _logger = logger;
            _iotHubProvisioningService = iotHubProvisioningService;
            _ioTDeviceRepo = ioTDeviceRepo;
        }

        public async Task OnMessage(AdminDSLInterpreterEndMessage message, object sender)
        {
            _logger.LogInformation($"Started execution {nameof(AdminDSLInterpreterEndMsgHandler)}.{nameof(OnMessage)}.");

            var addedDevices = await _iotHubProvisioningService.AddDevicesToIoTHubAsync(message.DeviceIds!);

            await _ioTDeviceRepo.BulkInsertIoTDevicesAsync(addedDevices.MapDevicesToIoTDeviceEntities());

            _logger.LogInformation($"Finished execution {nameof(AdminDSLInterpreterEndMsgHandler)}.{nameof(OnMessage)}.");
        }
    }
}
