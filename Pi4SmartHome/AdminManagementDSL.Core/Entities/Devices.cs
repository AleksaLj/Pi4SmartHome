
namespace AdminManagementDSL.Core.Entities
{
    public class Devices
    {
        public int DeviceId { get; set; }

        public char IsActive { get; set; }

        public DateTime? TimeActivated { get; set; }

        public DateTime? TimeDeactivated { get; set; }

        public int EstateId { get; set; }

        public int? EstatePartId { get; set; }

        public int DeviceTypeId { get; set; }
    }
}
