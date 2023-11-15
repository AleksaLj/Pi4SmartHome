
namespace DeviceManagement.Core.Entities
{
    public class IoTDevice
    {
        public int IoTDeviceId { get; set; }
        public byte DeviceStatus { get; set; }
        public byte ConnectionState { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public string IoTHubDeviceName { get; init; } = null!;
    }
}
