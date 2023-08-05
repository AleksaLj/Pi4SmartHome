using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Interpreter.Visitor
{
    public class CompoundElementNodeVisitor : INodeVisitor
    {
        private readonly INodeVisitor _visitor;

        public CompoundElementNodeVisitor(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task Visit(AST node, List<SqlTableDto> sqlTablesDto)
        {
            var compoundElement = (CompoundElementNode)node;

            var tableElements = compoundElement.TableElementNodes;

            foreach ( var tableElement in tableElements ) 
            {
                await _visitor.Visit(tableElement, sqlTablesDto);
            }
        }
    }
}
