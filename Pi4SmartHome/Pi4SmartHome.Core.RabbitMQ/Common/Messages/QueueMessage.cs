using System.Runtime.Serialization;
using System.Text.Json;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    [Serializable]
    public class QueueMessage : ISerializable
    {
        public Guid MessageId { get; protected set; }

        public QueueMessage()
        {
            MessageId = Guid.NewGuid();
        }

        protected QueueMessage(
            SerializationInfo info,
            StreamingContext context) 
        {
            if(string.IsNullOrEmpty(info.GetString(nameof(MessageId)))) throw new ArgumentNullException(nameof(MessageId));

            MessageId = Guid.Parse(info.GetString(nameof(MessageId))!);
        }

        public byte[] GetMessageData()
        { 
            using MemoryStream ms = new MemoryStream();
            JsonSerializer.Serialize(ms, this);

            return ms.ToArray();
        }

        public static TMessage? GetQueueMessage<TMessage>(byte[] data) where TMessage : QueueMessage
        {
            using MemoryStream ms = new MemoryStream(data);
            return GetQueueMessage<TMessage>(ms);
        }

        public static TMessage? GetQueueMessage<TMessage>(Stream stream) where TMessage : QueueMessage
        {
            var obj = JsonSerializer.Deserialize<TMessage>(stream);
            return obj;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(MessageId), MessageId.ToString());
        }
    }
}
