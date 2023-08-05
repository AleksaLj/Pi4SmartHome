using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Interpreter.Visitor
{
    public class ProvisionMainNodeVisitor : INodeVisitor
    {
        private readonly INodeVisitor _visitor;

        public ProvisionMainNodeVisitor(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task Visit(AST node, List<SqlTableDto> sqlTablesDto)
        {
            var provisionMain = (ProvisionMainNode)node;

            await _visitor.Visit(provisionMain.CompoundElement, sqlTablesDto);
        }
    }
}
