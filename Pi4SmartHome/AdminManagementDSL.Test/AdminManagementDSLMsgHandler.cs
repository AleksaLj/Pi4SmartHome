using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Core.Enums;
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

        public async Task OnMessage(AdminDSLMessage message, object sender)
        {
            Console.WriteLine("OnMessage in Consumer");
            Console.WriteLine($"Source code: {message.DslSourceCode}");

            //TO DO : Prepare message command and send to MediatR
            var createAdminDslCommand = new CreateAdminDSLCommand
            (
                DslCode: message.DslSourceCode,
                Status: DSLStatus.Queued
            );

            var isCreatedAdminDsl = await _mediator.Send(createAdminDslCommand);
        }
    }
}
