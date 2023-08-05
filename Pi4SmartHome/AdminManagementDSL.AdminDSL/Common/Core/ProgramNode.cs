using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class ProgramNode : AST
    {
        public AST ProvisionMainTree { get; set; }

        public ProgramNode(AST provisionMainTree)
        {
            ProvisionMainTree = provisionMainTree;
        }

        public override async Task Accept(INodeVisitor visitor, 
                                          List<SqlTableDto> sqlTablesDto) => await visitor.Visit(this, sqlTablesDto);
    }
}
