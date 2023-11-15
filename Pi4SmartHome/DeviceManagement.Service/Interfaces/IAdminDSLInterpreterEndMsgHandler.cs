using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace DeviceManagement.Service.Interfaces
{
    public interface IAdminDSLInterpreterEndMsgHandler : IMessageEventHandlerService<AdminDSLInterpreterEndMessage>
    {

    }
}
