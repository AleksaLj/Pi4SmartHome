using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Pi4SmartHome.Core.RabbitMQ.Common.Messages
{
    [Serializable]
    public class QueueMessage : EventArgs, ISerializable
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
            MessageId = Guid.Parse(info.GetString(nameof(MessageId))!);
        }

        public byte[] GetMessageData()
        { 
            var jsonString = JsonConvert.SerializeObject(this);
            var message = Encoding.UTF8.GetBytes(jsonString);

            return message;
        }

        public static TMessage? GetQueueMessage<TMessage>(byte[] data) where TMessage : QueueMessage
        {
            var message = Encoding.UTF8.GetString(data);
            return GetQueueMessage<TMessage>(message);
        }

        public static TMessage? GetQueueMessage<TMessage>(string? message) where TMessage : QueueMessage
        {
            if(message == null) return null;

            var obj = JsonConvert.DeserializeObject<TMessage>(message);
            return obj;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(MessageId), MessageId.ToString());
        }
    }
}
