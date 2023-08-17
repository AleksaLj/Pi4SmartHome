using System.Runtime.Serialization;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    public class AdminDSLMessage : QueueMessage
    {
        public string? DSLSourceCode { get; set; }

        public AdminDSLMessage(string? dslSourceCode)
        {
            DSLSourceCode = dslSourceCode ?? throw new ArgumentNullException(nameof(dslSourceCode));
        }

        protected AdminDSLMessage(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            DSLSourceCode = info.GetString(nameof(DSLSourceCode)) ?? throw new ArgumentNullException(nameof(DSLSourceCode));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(DSLSourceCode), DSLSourceCode);
        }
    }
}
