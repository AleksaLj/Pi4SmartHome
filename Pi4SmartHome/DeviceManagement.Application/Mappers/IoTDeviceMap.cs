using DeviceManagement.Core.Entities;
using Microsoft.Azure.Devices;

namespace DeviceManagement.Application.Mappers
{
    public static class IoTDeviceMap
    {
        public static IEnumerable<IoTDevice> MapDevicesToIoTDeviceEntities(this IEnumerable<Device> items)
        {
            var entities = items.Select(item => new IoTDevice
            {
                DeviceStatus = (byte)item.Status,
                ConnectionState = Convert.ToByte(item.ConnectionState),
                ActivationDate = DateTime.UtcNow,
                IoTHubDeviceName = item.Id
            });

            return entities;
        }
    }
}
