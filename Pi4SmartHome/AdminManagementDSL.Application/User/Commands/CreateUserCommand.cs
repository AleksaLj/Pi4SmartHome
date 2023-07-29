using MediatR;

namespace AdminManagementDSL.Application.User.Commands
{
    public record CreateUserCommand(string? firstName,
                                    string? lastName,
                                    DateTime? birthDate,
                                    string? addr,
                                    string? city,
                                    string? country,
                                    string email,
                                    string? phone,
                                    string pswrd,
                                    char gdprFlag,
                                    string signInKey,
                                    char emailVerify) : IRequest<int>;
}
