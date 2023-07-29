using MediatR;

namespace AdminManagementDSL.Application.Device.Commands
{
    public record CreateDeviceCommand(char isActive,
                                      DateTime? timeActivated,
                                      DateTime? timeDeactivated,
                                      int estateId,
                                      int? estatePartId,
                                      int deviceTypeId) : IRequest<int>;
}
