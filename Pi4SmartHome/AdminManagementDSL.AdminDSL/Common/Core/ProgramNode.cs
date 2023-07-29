using AdminManagementDSL.AdminDSL.Common.Interfaces;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class ProgramNode : AST
    {
        public AST ProvisionMainTree { get; set; }

        public ProgramNode(AST provisionMainTree)
        {
            ProvisionMainTree = provisionMainTree;
        }

        public override DataTable Accept(INodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
