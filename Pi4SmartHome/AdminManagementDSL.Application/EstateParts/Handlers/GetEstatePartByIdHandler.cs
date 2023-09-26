using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.EstateParts.Queries;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.EstateParts.Handlers
{
    public class GetEstatePartByIdHandler : IRequestHandler<GetEstatePartByIdQuery, EstatePart?>
    {
        private readonly IEstatePartRepo _estatePartRepo;

        public GetEstatePartByIdHandler(IEstatePartRepo estatePartRepo)
        {
            _estatePartRepo = estatePartRepo;
        }

        public async Task<EstatePart?> Handle(GetEstatePartByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _estatePartRepo.GetByIdAsync(request.Id);

            return item;
        }
    }
}
