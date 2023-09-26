using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.Estate.Queries;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.Estate.Handlers
{
    public class GetEstateByIdHandler : IRequestHandler<GetEstateByIdQuery, Estates?>
    {
        private readonly IEstatesRepo _estatesRepo;

        public GetEstateByIdHandler(IEstatesRepo estatesRepo)
        {
            _estatesRepo = estatesRepo;
        }

        public async Task<Estates?> Handle(GetEstateByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _estatesRepo.GetByIdAsync(request.Id);

            return item;
        }
    }
}
