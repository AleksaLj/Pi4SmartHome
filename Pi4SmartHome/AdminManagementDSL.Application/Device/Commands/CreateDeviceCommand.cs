using MediatR;

namespace AdminManagementDSL.Application.Device.Commands
{
    public record CreateDeviceCommand(char IsActive,
                                      DateTime? TimeActivated,
                                      DateTime? TimeDeactivated,
                                      int EstateId,
                                      int? EstatePartId,
                                      int DeviceTypeId) : IRequest<int>;
}
