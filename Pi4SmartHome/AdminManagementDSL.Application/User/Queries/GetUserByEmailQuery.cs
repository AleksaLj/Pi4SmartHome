using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.User.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<Users?>;
}
