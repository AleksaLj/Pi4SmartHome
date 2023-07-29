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
            //pre-checks and validations;

            var item = new Devices
            {
                IsActive = request.isActive,
                TimeActivated = request.timeActivated,
                TimeDeactivated = request.timeDeactivated,
                EstateId = request.estateId,
                EstatePartId = request.estatePartId,
                DeviceTypeId = request.deviceTypeId
            };

            return await _devicesRepo.InsertAsync(item);
        }
    }
}
