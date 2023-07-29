using AdminManagementDSL.AdminDSL.Common.Interfaces;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class ProvisionMainNode : AST
    {
        public AST CompoundElement { get; set; }

        public ProvisionMainNode(AST compoundElement)
        {
            CompoundElement = compoundElement;
        }

        public override DataTable Accept(INodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
