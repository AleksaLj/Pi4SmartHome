using System.Runtime.Serialization;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    [Serializable]
    public class AdminDSLMessage : QueueMessage
    {
        public string? DslSourceCode { get; set; }

        public AdminDSLMessage(string? dslSourceCode)
        {
            DslSourceCode = dslSourceCode;
        }

        protected AdminDSLMessage(
            SerializationInfo info, 
            StreamingContext context) : base(info, context)
        {
            DslSourceCode = info.GetString(nameof(DslSourceCode)) ?? throw new ArgumentNullException(info.GetString(nameof(DslSourceCode)));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(DslSourceCode), DslSourceCode);
        }
    }
}
