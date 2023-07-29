using MediatR;

namespace AdminManagementDSL.Application.User.Commands
{
    public record VerifyEmailCommand(string email) : IRequest<int>;
}
