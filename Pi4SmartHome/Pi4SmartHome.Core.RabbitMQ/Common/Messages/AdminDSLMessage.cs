﻿using System.Runtime.Serialization;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    [Serializable]
    public class AdminDSLMessage : QueueMessage
    {
        public string? DslSourceCode { get; set; }
        public Guid AdminDslGuid { get; set; }

        public AdminDSLMessage(string? dslSourceCode, Guid adminDslGuid)
        {
            DslSourceCode = dslSourceCode;
            AdminDslGuid = adminDslGuid;
        }

        protected AdminDSLMessage(
            SerializationInfo info, 
            StreamingContext context) : base(info, context)
        {
            DslSourceCode = info.GetString(nameof(DslSourceCode)) ?? throw new ArgumentNullException(info.GetString(nameof(DslSourceCode)));
            AdminDslGuid = Guid.Parse(info.GetString(nameof(AdminDslGuid)) ?? throw new ArgumentNullException(info.GetString(nameof(AdminDslGuid))));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(DslSourceCode), DslSourceCode);
            info.AddValue(nameof(AdminDslGuid), AdminDslGuid);
        }
    }
}
