using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Queries
{
    public record GetAdminDSLByIdQuery(int Id) : IRequest<Core.Entities.AdminDSL?>;
}
