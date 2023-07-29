using AdminManagementDSL.AdminDSL.Common.Core;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Common.Interfaces
{
    public interface INodeVisitor
    {
        DataTable Visit(AST node);
    }
}
