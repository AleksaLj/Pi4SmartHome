using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.Device.Commands;
using AdminManagementDSL.Core.Entities;
using MediatR;
namespace AdminManagementDSL.Application.Device.Handlers
{
    public class CreateDeviceHandler : IRequestHandler<CreateDeviceCommand, int>
    {
        private readonly IDevicesRepo _devicesRepo;

        public CreateDeviceHandler(IDevicesRepo devicesRepo)
        {
            _devicesRepo = devicesRepo;
        }

        public async Task<int> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            var item = new Devices
            {
                IsActive = request.IsActive,
                TimeActivated = request.TimeActivated,
                TimeDeactivated = request.TimeDeactivated,
                EstateId = request.EstateId,
                EstatePartId = request.EstatePartId,
                DeviceTypeId = request.DeviceTypeId
            };

            return await _devicesRepo.InsertAsync(item);
        }
    }
}
