using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.Device.Queries
{
    public record GetDeviceByIdQuery(int Id) : IRequest<Devices?>;
}
