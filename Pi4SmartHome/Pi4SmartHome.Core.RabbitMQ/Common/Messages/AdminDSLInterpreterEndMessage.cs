
using System.Runtime.Serialization;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    [Serializable]
    public class AdminDSLInterpreterEndMessage : QueueMessage
    {
        public Guid AdminDslGuid { get; set; }

        public AdminDSLInterpreterEndMessage(Guid adminDslGuid)
        {
            AdminDslGuid = adminDslGuid;
        }

        protected AdminDSLInterpreterEndMessage(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            AdminDslGuid = Guid.Parse(info.GetString(nameof(AdminDslGuid)) ?? throw new ArgumentNullException(info.GetString(nameof(AdminDslGuid))));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(AdminDslGuid), AdminDslGuid);
        }
    }
}
