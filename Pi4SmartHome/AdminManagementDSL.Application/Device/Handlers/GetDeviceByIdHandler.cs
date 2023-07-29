using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.Device.Queries;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.Device.Handlers
{
    public class GetDeviceByIdHandler : IRequestHandler<GetDeviceByIdQuery, Devices?>
    {
        private readonly IDevicesRepo _devicesRepo;

        public GetDeviceByIdHandler(IDevicesRepo devicesRepo)
        {
            _devicesRepo = devicesRepo;
        }

        public async Task<Devices?> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _devicesRepo.GetByIdAsync(request.id);

            return item;
        }
    }
}
