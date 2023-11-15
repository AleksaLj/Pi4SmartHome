using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.Common.Models;
using AdminManagementDSL.Application.Device.Queries;
using MediatR;

namespace AdminManagementDSL.Application.Device.Handlers
{
    public class GetDevicesForIoTHubHandler : IRequestHandler<GetDevicesForIoTHubQuery, IEnumerable<AddIoTDeviceModel>>
    {
        private readonly IDevicesRepo _devicesRepo;

        public GetDevicesForIoTHubHandler(IDevicesRepo devicesRepo)
        {
            _devicesRepo = devicesRepo;
        }

        public async Task<IEnumerable<AddIoTDeviceModel>> Handle(GetDevicesForIoTHubQuery request, CancellationToken cancellationToken)
        {
            var items = await _devicesRepo.GetDevicesForIoTHubImportAsync(request.AdminDslGuid);

            return items;
        }
    }
}
