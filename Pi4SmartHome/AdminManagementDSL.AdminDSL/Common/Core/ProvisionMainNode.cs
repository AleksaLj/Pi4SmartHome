using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class ProvisionMainNode : AST
    {
        public AST CompoundElement { get; set; }

        public ProvisionMainNode(AST compoundElement)
        {
            CompoundElement = compoundElement;
        }

        public override async Task Accept(INodeVisitor visitor, 
                                          List<SqlTableDto> sqlTablesDto) => await visitor.Visit(this, sqlTablesDto);
    }
}
