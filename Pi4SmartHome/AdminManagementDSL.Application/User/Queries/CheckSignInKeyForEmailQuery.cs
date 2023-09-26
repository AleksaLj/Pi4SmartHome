using MediatR;

namespace AdminManagementDSL.Application.User.Queries
{
    public record CheckSignInKeyForEmailQuery(string Email, string SignInKey) : IRequest<bool>;
}
