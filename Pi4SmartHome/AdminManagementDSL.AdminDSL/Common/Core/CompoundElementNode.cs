using AdminManagementDSL.AdminDSL.Common.Interfaces;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class CompoundElementNode : AST
    {
        public List<AST> TableElementNodes { get; set; }

        public CompoundElementNode()
        {
            TableElementNodes = new List<AST>();
        }

        public CompoundElementNode(List<AST> tableElementNodes)
        {
            TableElementNodes = tableElementNodes;
        }

        public override DataTable Accept(INodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
