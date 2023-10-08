using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Application.Common.Interfaces;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Handlers
{
    public class CreateAdminDSLHandler : IRequestHandler<CreateAdminDSLCommand, int>
    {
        private readonly IAdminDSLRepo _adminDslRepo;

        public CreateAdminDSLHandler(IAdminDSLRepo adminDslRepo)
        {
            _adminDslRepo = adminDslRepo;
        }

        public async Task<int> Handle(CreateAdminDSLCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(request.DslCode))
            {
                throw new ArgumentException("Parameter 'DslCode' cannot be null or empty");
            }

            var item = new Core.Entities.AdminDSL
            {
                DSLCode = request.DslCode,
                DSLStatus = (byte)request.Status,
                AdminDSLGuid = request.DslGuid
            };

            return await _adminDslRepo.InsertAsync(item);
        }
    }
}
