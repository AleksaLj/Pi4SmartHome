using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using Pi4SmartHome.Core.Helper;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Interpreter
{
    public class AdminDSLInterpreter : IAdminDSLInterpreter
    {
        private readonly INodeVisitor _visitor;

        public AdminDSLInterpreter(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task<IEnumerable<SqlTableDto>> Interpret(AST tree)
        {
            var program = (ProgramNode)tree;

            var sqlTablesDto = new List<SqlTableDto>();
            await _visitor.Visit(program, sqlTablesDto);

            return sqlTablesDto;
        }
    }
}
