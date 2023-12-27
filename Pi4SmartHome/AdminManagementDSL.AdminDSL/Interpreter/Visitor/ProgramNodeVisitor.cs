using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Interpreter.Visitor
{
    public class ProgramNodeVisitor : INodeVisitor
    {
        private readonly INodeVisitor _visitor;

        public ProgramNodeVisitor(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task Visit(AST node, List<SqlTableDto> sqlTablesDto)
        {
            var program = (ProgramNode)node;

            await _visitor.Visit(program.ProvisionMainTree, sqlTablesDto);
;        }
    }
}
