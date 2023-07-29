using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.Estate.Commands;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.Estate.Handlers
{
    public class CreateEstateHandler : IRequestHandler<CreateEstateCommand, int>
    {
        private readonly IEstatesRepo _estatesRepo;

        public CreateEstateHandler(IEstatesRepo estatesRepo)
        {
            _estatesRepo = estatesRepo;
        }

        public async Task<int> Handle(CreateEstateCommand request, CancellationToken cancellationToken)
        {
            //pre-checks and validation

            var item = new Estates
            {
                Addr = request.addr,
                Description = request.description,
                EstateTypeId = request.estateTypeId
            };

            return await _estatesRepo.InsertAsync(item);
        }
    }
}
