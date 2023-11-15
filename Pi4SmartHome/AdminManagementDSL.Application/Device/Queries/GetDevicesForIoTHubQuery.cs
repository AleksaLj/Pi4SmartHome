using AdminManagementDSL.Application.Common.Models;
using MediatR;

namespace AdminManagementDSL.Application.Device.Queries
{
    public record GetDevicesForIoTHubQuery(string AdminDslGuid) : IRequest<IEnumerable<AddIoTDeviceModel>>;
}
