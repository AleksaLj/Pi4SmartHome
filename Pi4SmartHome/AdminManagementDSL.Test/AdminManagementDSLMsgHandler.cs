using System.Text;
using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using AdminManagementDSL.AdminDSL.Scanner;
using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Application.AdminDSL.Queries;
using AdminManagementDSL.Core.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace AdminManagementDSL.Test
{
    public class AdminManagementDSLMsgHandler : IMessageEventHandlerService<AdminDSLMessage>
    {
        private readonly IMediator _mediator;
        private readonly IAdminDSLParser _parser;
        private readonly IAdminDSLInterpreter _interpreter;
        private readonly ILogger<AdminManagementDSLMsgHandler> _logger;

        public AdminManagementDSLMsgHandler(IMediator mediator, 
                                            IAdminDSLParser parser,
                                            IAdminDSLInterpreter interpreter,
                                            ILogger<AdminManagementDSLMsgHandler> logger)
        {
            _mediator = mediator;
            _parser = parser;
            _interpreter = interpreter;
            _logger = logger;
        }

        public async Task OnMessage(AdminDSLMessage message, object sender)
        {
            var createAdminDslCommand = new CreateAdminDSLCommand
            (
                DslCode: message.DslSourceCode,
                DslGuid: message.AdminDslGuid,
                Status: DSLStatus.Queued
            );

            var createdAdminDslId = await _mediator.Send(createAdminDslCommand);

            if (createdAdminDslId > 0)
                await InterpretAdminDSLMessage(createdAdminDslId);
        }

        private async Task InterpretAdminDSLMessage(int createdAdminDslId)
        {
            var adminDsl = await GetAdminDslByIdAsync(createdAdminDslId) ?? 
                           throw new ArgumentNullException(nameof(createdAdminDslId));
            if (string.IsNullOrEmpty(adminDsl.DSLCode))
            {
                throw new NullReferenceException(nameof(adminDsl.DSLCode));
            }

            await UpdateAdminDslStatusAsync(adminDsl, DSLStatus.Processing);

            var dslCode = GetAdminDslCodeFromJson(adminDsl.DSLCode);
            IAdminDSLScanner scanner = new AdminDSLScanner();
            await scanner.Configure(dslCode);
            var tree = await _parser.Parse(scanner);
            var sqlTables = await _interpreter.Interpret(tree);

            var result = await CreatePi4SmartHomeInterpredDataAsync(sqlTables, adminDsl.AdminDSLId);

            if (result)
            {
                _logger.LogInformation("Successfully interpreted message: {Message}", adminDsl.DSLCode);
                await UpdateAdminDslStatusAsync(adminDsl, DSLStatus.Finished);
            }
            else
            {
                await UpdateAdminDslStatusAsync(adminDsl, DSLStatus.Error);
            }
        }

        private async Task<bool> CreatePi4SmartHomeInterpredDataAsync(IEnumerable<SqlTableDto> sqlTables, int adminDslId)
        {
            var createPi4SmartHomeInterpretedDataCommand =
                new CreatePi4SmartHomeInterpretedDataCommand(SqlTables: sqlTables, AdminDslId: adminDslId);

            var result = await _mediator.Send(createPi4SmartHomeInterpretedDataCommand);

            return result;
        }

        private async Task<Core.Entities.AdminDSL?> GetAdminDslByIdAsync(int adminDslId)
        {
            var getAdminDslByIdQuery = new GetAdminDSLByIdQuery
            (
                Id: adminDslId
            );

            return await _mediator.Send(getAdminDslByIdQuery);
        }

        private async Task UpdateAdminDslStatusAsync(Core.Entities.AdminDSL? adminDsl, DSLStatus status)
        {
            if(adminDsl == null)
                throw new ArgumentNullException(nameof(adminDsl));

            if(adminDsl.DSLStatus is (byte)DSLStatus.Finished or (byte)DSLStatus.Error)
                return;

            var updateAdminDslStatusCommand = new UpdateAdminDSLStatusCommand
            (
                AdminDslId: adminDsl.AdminDSLId,
                Status: status
            );

            await _mediator.Send(updateAdminDslStatusCommand);
        }

        private static string GetAdminDslCodeFromJson(string adminDslJson)
        {
            var adminDslBuilder = new StringBuilder(adminDslJson);

            adminDslBuilder.Remove(0, 1);
            adminDslBuilder.Remove(adminDslBuilder.Length - 1, 1);
            adminDslBuilder.Replace("\"DSLSourceCode\":", "");
            adminDslBuilder.Remove(0, 1);
            adminDslBuilder.Remove(adminDslBuilder.Length - 1, 1);

            return adminDslBuilder.ToString();
        }
    }
}
