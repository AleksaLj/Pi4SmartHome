using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

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

        public override async Task Accept(INodeVisitor visitor, 
                                          List<SqlTableDto> sqlTablesDto) => await visitor.Visit(this, sqlTablesDto);
    }
}
