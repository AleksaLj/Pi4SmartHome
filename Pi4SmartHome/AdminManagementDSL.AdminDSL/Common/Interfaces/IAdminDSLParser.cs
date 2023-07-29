using AdminManagementDSL.AdminDSL.Common.Core;

namespace AdminManagementDSL.AdminDSL.Common.Interfaces
{
    public interface IAdminDSLParser
    {
        Task<AST> Parse(IAdminDSLScanner scanner);
    }
}
