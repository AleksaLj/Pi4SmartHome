using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Commands;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, int>
    {
        private readonly IUsersRepo _usersRepo;

        public VerifyEmailHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<int> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await _usersRepo.VerifyEmailAsync(request.email);

            return result;
        }
    }
}
