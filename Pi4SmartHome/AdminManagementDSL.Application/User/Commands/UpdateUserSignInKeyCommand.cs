using MediatR;

namespace AdminManagementDSL.Application.User.Commands
{
    public record UpdateUserSignInKeyCommand(string Email, string SignInKey) : IRequest<int>;
}
