using AdminManagementDSL.Core.Enums;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Commands
{
    public record CreateAdminDSLCommand(string? DslCode,
                                        DSLStatus Status) : IRequest<int>;
}
