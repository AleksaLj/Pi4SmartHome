
using AdminManagementDSL.AdminDSL.Common.Core;

namespace AdminManagementDSL.AdminDSL.Common.Interfaces
{
    public interface IAdminDSLScanner
    {
        Task Configure(string programCode);
        Token? CurrentToken { get; set; }
        Task<Token?> GetNextToken();
    }
}
