
using AdminManagementDSL.Core.Enums;

namespace AdminManagementDSL.Application.Common.Interfaces
{
    public interface IAdminDSLRepo : IRepository<Core.Entities.AdminDSL>
    {
        Task<int> UpdateAdminDSLStatusAsync(int adminDslId, DSLStatus status);
    }
}
