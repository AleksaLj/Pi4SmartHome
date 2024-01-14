using Pi4SmartHomeDSL.Application.Models;

namespace Pi4SmartHomeDSL.Application.Interfaces
{
    public interface IIoTHubCloudToDeviceMessagingService
    {
        Task SendMessageToDeviceAsync(IoTDeviceMessage message);
    }
}
