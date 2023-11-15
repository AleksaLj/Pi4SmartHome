using DeviceManagement.Core.Entities;

namespace DeviceManagement.Application.Interfaces
{
    public interface IIoTDeviceRepo : IInsertRepo<IoTDevice>
    {
        Task<int> BulkInsertIoTDevicesAsync(IEnumerable<IoTDevice> items);
    }
}
