using MediatR;

namespace AdminManagementDSL.Application.User.Commands
{
    public record CreateUserCommand(string? FirstName,
                                    string? LastName,
                                    DateTime? BirthDate,
                                    string? Addr,
                                    string? City,
                                    string? Country,
                                    string Email,
                                    string? Phone,
                                    string Pswrd,
                                    char GdprFlag,
                                    string SignInKey,
                                    char EmailVerify) : IRequest<int>;
}
