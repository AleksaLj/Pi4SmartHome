
using System.Runtime.Serialization;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    [Serializable]
    public class CloudToDeviceMessage : QueueMessage
    {
        public string? CloudToDeviceMessageSourceCode { get; set; }

        public CloudToDeviceMessage(string? cloudToDeviceMessageSourceCode)
        {
            CloudToDeviceMessageSourceCode = cloudToDeviceMessageSourceCode;
        }

        protected CloudToDeviceMessage(
            SerializationInfo info,
            StreamingContext context)
        {
            CloudToDeviceMessageSourceCode = info.GetString(nameof(CloudToDeviceMessageSourceCode)) ?? 
                                             throw new ArgumentNullException(info.GetString(nameof(CloudToDeviceMessageSourceCode)));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(CloudToDeviceMessageSourceCode), CloudToDeviceMessageSourceCode);
        }
    }
}
