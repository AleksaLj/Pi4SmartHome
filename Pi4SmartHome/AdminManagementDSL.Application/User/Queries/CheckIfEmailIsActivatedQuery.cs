using MediatR;

namespace AdminManagementDSL.Application.User.Queries
{
    public record CheckIfEmailIsActivatedQuery(string Email) : IRequest<bool>;
}
