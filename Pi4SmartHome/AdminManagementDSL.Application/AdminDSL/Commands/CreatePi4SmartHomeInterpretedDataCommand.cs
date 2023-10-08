using AdminManagementDSL.AdminDSL.Common.Dto;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Commands
{
    public record CreatePi4SmartHomeInterpretedDataCommand(IEnumerable<SqlTableDto> SqlTables, int AdminDslId) : IRequest<bool>;
}
