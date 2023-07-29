using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.User.Queries
{
    public record GetUsersQuery() : IRequest<IEnumerable<Users>>;
}
