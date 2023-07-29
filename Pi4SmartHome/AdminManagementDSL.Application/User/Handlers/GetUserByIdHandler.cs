using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Queries;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Users?>
    {
        private readonly IUsersRepo _usersRepo;

        public GetUserByIdHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<Users?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _usersRepo.GetByIdAsync(request.id);

            return item;
        }
    }
}
