using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Queries;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class CheckSignInKeyForEmailHandler : IRequestHandler<CheckSignInKeyForEmailQuery, bool>
    {
        private readonly IUsersRepo _usersRepo;

        public CheckSignInKeyForEmailHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<bool> Handle(CheckSignInKeyForEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await _usersRepo.CheckSignInKeyForEmailAsync(request.email, request.signInKey);

            return result;
        }
    }
}
