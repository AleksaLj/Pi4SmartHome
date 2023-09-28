using AdminManagementDSL.AdminDSL.Common.Interfaces;
using AdminManagementDSL.AdminDSL.Scanner;
using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Application.AdminDSL.Queries;
using AdminManagementDSL.Core.Entities;
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
        private readonly IAdminDSLParser _parser;
        private readonly IAdminDSLInterpreter _interpreter;

        public AdminManagementDSLMsgHandler(IMediator mediator, 
                                            IAdminDSLParser parser,
                                            IAdminDSLInterpreter interpreter)
        {
            _mediator = mediator;
            _parser = parser;
            _interpreter = interpreter;
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

            var createdAdminDslId = await _mediator.Send(createAdminDslCommand);

            if (createdAdminDslId > 0)
                InterpretAdminDSLMessage(createdAdminDslId);
        }

        private async Task InterpretAdminDSLMessage(int createdAdminDslId)
        {

            var adminDsl = await GetAdminDSLByIdAsync(createdAdminDslId) ?? 
                           throw new ArgumentNullException("Cannot interpret null parameter.");
            if (string.IsNullOrEmpty(adminDsl.DSLCode))
            {
                throw new ArgumentNullException("Parameter 'DSLCode' cannot be null or empty.");
            }

            //TO DO : before interpreting check state machine and update status in db.

            IAdminDSLScanner scanner = new AdminDSLScanner();
            await scanner.Configure(adminDsl.DSLCode);
            var tree = await parser.Parse(scanner);
            var sqlTables = await interpreter.Interpret(tree);

            //TO DO : insert into sql tables - check in db.
        }

        private async Task<Core.Entities.AdminDSL?> GetAdminDSLByIdAsync(int adminDslId)
        {
            var getAdminDSLByIdQuery = new GetAdminDSLByIdQuery
            (
                Id: adminDslId
            );

            return await _mediator.Send(getAdminDSLByIdQuery);
        }
    }
}
