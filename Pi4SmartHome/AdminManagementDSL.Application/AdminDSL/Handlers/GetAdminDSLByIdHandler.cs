using AdminManagementDSL.Application.AdminDSL.Queries;
using AdminManagementDSL.Application.Common.Interfaces;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Handlers
{
    public class GetAdminDSLByIdHandler : IRequestHandler<GetAdminDSLByIdQuery, Core.Entities.AdminDSL?>
    {
        private readonly IAdminDSLRepo _adminDslRepo;

        public GetAdminDSLByIdHandler(IAdminDSLRepo adminDslRepo)
        {
            _adminDslRepo = adminDslRepo;
        }

        public async Task<Core.Entities.AdminDSL?> Handle(GetAdminDSLByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentException("Null parameter exception");
            }

            if (request.Id <= 0)
            {
                throw new ArgumentException("Parameter 'Id' cannot be null or empty!");
            }

            var item = await _adminDslRepo.GetByIdAsync(request.Id);

            return item;
        }
    }
}
