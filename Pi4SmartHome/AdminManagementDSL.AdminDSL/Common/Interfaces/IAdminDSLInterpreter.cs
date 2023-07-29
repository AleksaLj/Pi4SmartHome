using AdminManagementDSL.AdminDSL.Common.Core;

namespace AdminManagementDSL.AdminDSL.Common.Interfaces
{
    public interface IAdminDSLInterpreter
    {
        Task Interpret(AST tree);
    }
}
