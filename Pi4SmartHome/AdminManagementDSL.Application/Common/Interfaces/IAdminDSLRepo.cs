using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.Core.Enums;

namespace AdminManagementDSL.Application.Common.Interfaces
{
    public interface IAdminDSLRepo : IRepository<Core.Entities.AdminDSL>
    {
        Task<int> UpdateAdminDSLStatusAsync(int adminDslId, DSLStatus status);
        Task BulkInsertPi4SmartHomeInterpretedDataAsync(IEnumerable<SqlTableDto> sqlTables, int adminDslId);
    }
}
