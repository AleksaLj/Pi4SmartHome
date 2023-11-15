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
            return node switch
            {
                ProgramNode => node.Accept(new ProgramNodeVisitor(this), sqlTablesDto),
                ProvisionMainNode => node.Accept(new ProvisionMainNodeVisitor(this), sqlTablesDto),
                CompoundElementNode => node.Accept(new CompoundElementNodeVisitor(this), sqlTablesDto),
                TableElementNode => node.Accept(new TableElementNodeVisitor(this), sqlTablesDto),
                _ => throw Error.ErrorMessages.NonexistentVisitorMethodErr()
            };
        }
    }
}
