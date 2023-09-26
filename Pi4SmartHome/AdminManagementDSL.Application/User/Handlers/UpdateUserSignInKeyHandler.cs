using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Commands;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class UpdateUserSignInKeyHandler : IRequestHandler<UpdateUserSignInKeyCommand, int>
    {
        private readonly IUsersRepo _usersRepo;

        public UpdateUserSignInKeyHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<int> Handle(UpdateUserSignInKeyCommand request, CancellationToken cancellationToken)
        {
            var result = await _usersRepo.UpdateUserSignInKeyAsync(request.Email, request.SignInKey);

            return result;
        }
    }
}
