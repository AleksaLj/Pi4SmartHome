using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;

namespace AdminManagementDSL.AdminDSL.Common.Interfaces
{
    public interface IAdminDSLInterpreter
    {
        Task<IEnumerable<SqlTableDto>> Interpret(AST tree);
    }
}
