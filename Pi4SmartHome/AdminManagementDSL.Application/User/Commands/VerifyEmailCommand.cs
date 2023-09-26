using MediatR;

namespace AdminManagementDSL.Application.User.Commands
{
    public record VerifyEmailCommand(string Email) : IRequest<int>;
}
