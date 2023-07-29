using MediatR;

namespace AdminManagementDSL.Application.User.Queries
{
    public record CheckSignInKeyForEmailQuery(string email, string signInKey) : IRequest<bool>;
}
