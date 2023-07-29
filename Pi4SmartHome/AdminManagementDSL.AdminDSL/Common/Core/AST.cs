using AdminManagementDSL.AdminDSL.Common.Interfaces;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public abstract class AST
    {
        public abstract DataTable Accept(INodeVisitor visitor);
    }
}
