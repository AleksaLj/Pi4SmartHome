using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace CloudToDevice.Service.Interfaces
{
    public interface ICloudToDeviceMessageHandler : IMessageEventHandlerService<CloudToDeviceMessage>
    {

    }
}
