using Microsoft.Azure.Devices;

namespace DeviceManagement.Application.Interfaces
{
    public interface IIoTHubDeviceProvisioningService
    {
        Task<IEnumerable<Device>> AddDevicesToIoTHubAsync(IEnumerable<string> deviceIds);
    }
}
