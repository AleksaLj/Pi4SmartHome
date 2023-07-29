using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Queries;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class CheckIfEmailIsActivatedHandler : IRequestHandler<CheckIfEmailIsActivatedQuery, bool>
    {
        private readonly IUsersRepo _usersRepo;

        public CheckIfEmailIsActivatedHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<bool> Handle(CheckIfEmailIsActivatedQuery request, CancellationToken cancellationToken)
        {
            var result = await _usersRepo.CheckIfEmailIsActivatedAsync(request.email);

            return result;
        }
    }
}
