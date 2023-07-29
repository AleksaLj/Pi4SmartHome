using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Queries;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, Users?>
    {
        private readonly IUsersRepo _usersRepo;

        public GetUserByEmailHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<Users?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var item = await _usersRepo.SelectUserByEmailAsync(request.email);

            return item;
        }
    }
}
