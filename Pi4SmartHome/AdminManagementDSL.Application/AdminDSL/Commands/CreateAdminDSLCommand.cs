using AdminManagementDSL.Core.Enums;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Commands
{
    public record CreateAdminDSLCommand(string? DslCode,
                                        Guid DslGuid,
                                        DSLStatus Status) : IRequest<int>;
}
