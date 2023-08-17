using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Exceptions;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Interpreter.Visitor
{
    public class NodeVisitor : INodeVisitor
    {
        public Task Visit(AST node, List<SqlTableDto> sqlTablesDto)
        {
            if (node is ProgramNode)
            {
                return node.Accept(new ProgramNodeVisitor(this), sqlTablesDto);
            }
            if (node is ProvisionMainNode)
            {
                return node.Accept(new ProvisionMainNodeVisitor(this), sqlTablesDto);
            }
            if (node is CompoundElementNode)
            {
                return node.Accept(new CompoundElementNodeVisitor(this), sqlTablesDto);
            }
            if (node is TableElementNode)
            {
                return node.Accept(new TableElementNodeVisitor(this), sqlTablesDto);
            }

            throw Error.ErrorMessages.NonexistentVisitorMethodErr();
        }
    }
}
