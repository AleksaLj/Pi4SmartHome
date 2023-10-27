using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementDSL.Service.Interfaces
{
    public interface IAdminManagementDSLMsgHandler : IMessageEventHandlerService<AdminDSLMessage>
    {
        Task OnMessage(AdminDSLMessage message, object sender);
    }
}
