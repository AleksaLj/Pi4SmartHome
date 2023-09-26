using MediatR;
using Pi4SmartHome.Core.Helper;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementDSL.Test
{
    public class AdminManagementDSLMsgHandler : IMessageEventHandlerService<AdminDSLMessage>
    {
        private readonly IMediator _mediator;

        public AdminManagementDSLMsgHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task OnMessage(AdminDSLMessage message, object sender)
        {
            Console.WriteLine("OnMessage in Consumer");
            Console.WriteLine($"Source code: {message.DslSourceCode}");

            //TO DO : Prepare message command and send to MediatR
            

            return TaskCache.True;
        }
    }
}
