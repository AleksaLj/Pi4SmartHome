using DeviceManagement.Application.Models;

namespace DeviceManagement.Application.Interfaces
{
    public interface IIoTHubCloudToDeviceMessagingService
    {
        Task SendMessageToDeviceAsync(IoTDeviceMessage message);
    }
}
