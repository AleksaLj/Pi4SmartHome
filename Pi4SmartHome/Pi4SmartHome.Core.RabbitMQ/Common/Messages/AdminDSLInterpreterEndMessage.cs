
using System.Runtime.Serialization;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    [Serializable]
    public class AdminDSLInterpreterEndMessage : QueueMessage
    {
        public Guid AdminDslGuid { get; set; }
        public IEnumerable<string>? DeviceIds { get; set; }

        public AdminDSLInterpreterEndMessage(Guid adminDslGuid, IEnumerable<string>? deviceIds)
        {
            AdminDslGuid = adminDslGuid;
            DeviceIds = deviceIds;
        }

        protected AdminDSLInterpreterEndMessage(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            AdminDslGuid = Guid.Parse(info.GetString(nameof(AdminDslGuid)) ?? throw new ArgumentNullException(info.GetString(nameof(AdminDslGuid))));
            DeviceIds = (IEnumerable<string>?)info.GetValue(nameof(DeviceIds), typeof(IEnumerable<string>)) ?? throw new ArgumentNullException(info.GetString(nameof(DeviceIds)));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(AdminDslGuid), AdminDslGuid);
            info.AddValue(nameof(DeviceIds), DeviceIds);
        }
    }
}
