using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.EstateParts.Commands;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.EstateParts.Handlers
{
    public class CreateEstatePartHandler : IRequestHandler<CreateEstatePartCommand, int>
    {
        private readonly IEstatePartRepo _estatePartRepo;

        public CreateEstatePartHandler(IEstatePartRepo estatePartRepo)
        {
            _estatePartRepo = estatePartRepo;
        }

        public async Task<int> Handle(CreateEstatePartCommand request, CancellationToken cancellationToken)
        {
            //pre-checks and validations;

            var item = new EstatePart
            {
                EstatePartName = request.estatePartName,
                EstateId = request.estateId
            };

            return await _estatePartRepo.InsertAsync(item);
        }
    }
}
