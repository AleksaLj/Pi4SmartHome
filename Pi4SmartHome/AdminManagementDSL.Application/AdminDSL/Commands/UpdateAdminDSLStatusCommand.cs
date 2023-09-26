using AdminManagementDSL.Core.Enums;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Commands
{
    public record UpdateAdminDSLStatusCommand(int AdminDslId,
                                              DSLStatus Status) : IRequest<int>;
}
