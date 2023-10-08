using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Application.Common.Interfaces;
using MediatR;

namespace AdminManagementDSL.Application.AdminDSL.Handlers
{
    public class CreatePi4SmartHomeInterpretedDataHandler : IRequestHandler<CreatePi4SmartHomeInterpretedDataCommand, bool>
    {
        private readonly IAdminDSLRepo _adminDslRepo;

        public CreatePi4SmartHomeInterpretedDataHandler(IAdminDSLRepo adminDslRepo)
        {
            _adminDslRepo = adminDslRepo;
        }

        public async Task<bool> Handle(CreatePi4SmartHomeInterpretedDataCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.AdminDslId <= 0)
            {
                throw new ArgumentException("Parameter 'AdminDslId' cannot be less or equal zero");
            }

            if (!request.SqlTables.Any())
            {
                throw new ArgumentException("Empty table list!");
            }

            await _adminDslRepo.BulkInsertPi4SmartHomeInterpretedDataAsync(request.SqlTables, request.AdminDslId);

            return true;
        }
    }
}
