using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DeviceManagement.Application.Models
{
    [Serializable]
    public class IoTDeviceMessage : ISerializable
    {
        public string DeviceId { get; init; }
        public string? MessageBody { get; set; }
        public Dictionary<string, string> MessageProperties { get; set; }

        public IoTDeviceMessage(string deviceId, 
                                string? messageBody, 
                                Dictionary<string, string> messageProperties)
        {
            DeviceId = deviceId;
            MessageBody = messageBody;
            MessageProperties = messageProperties;
        }

        public byte[] GetIoTDeviceMessageData()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            var message = Encoding.UTF8.GetBytes(jsonString);

            return message;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(MessageBody), MessageBody);
            info.AddValue(nameof(MessageProperties), MessageProperties);
        }
    }
}
