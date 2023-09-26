using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Application.Common.Interfaces;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Handlers
{
    public class UpdateAdminDSLStatusHandler : IRequestHandler<UpdateAdminDSLStatusCommand, int>
    {
        private readonly IAdminDSLRepo _adminDslRepo;

        public UpdateAdminDSLStatusHandler(IAdminDSLRepo adminDslRepo)
        {
            _adminDslRepo = adminDslRepo;
        }

        public async Task<int> Handle(UpdateAdminDSLStatusCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.AdminDslId <= 0)
            {
                throw new ArgumentException("Invalid parameter 'AdminDslId'!");
            }

            var result = await _adminDslRepo.UpdateAdminDSLStatusAsync(request.AdminDslId, request.Status);

            return result;
        }
    }
}
