using MediatR;

namespace AdminManagementDSL.Application.User.Commands
{
    public record UpdateUserSignInKeyCommand(string email, string signInKey) : IRequest<int>;
}
