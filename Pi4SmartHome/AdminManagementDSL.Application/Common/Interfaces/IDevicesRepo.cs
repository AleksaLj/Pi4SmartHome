using AdminManagementDSL.Application.Common.Models;
using AdminManagementDSL.Core.Entities;

namespace AdminManagementDSL.Application.Common.Interfaces
{
    public interface IDevicesRepo : IRepository<Devices>
    {
        Task<IEnumerable<AddIoTDeviceModel>> GetDevicesForIoTHubImportAsync(string adminDslGuid);
    }
}
