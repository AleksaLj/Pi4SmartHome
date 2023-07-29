using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.User.Queries
{
    public record GetUserByIdQuery(int id) : IRequest<Users?>;
}
