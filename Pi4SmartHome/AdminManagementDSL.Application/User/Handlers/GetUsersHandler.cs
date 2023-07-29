using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Queries;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<Users>>
    {
        private readonly IUsersRepo _usersRepo;

        public GetUsersHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<IEnumerable<Users>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var items = await _usersRepo.GetAllAsync();

            return items;
        }
    }
}
