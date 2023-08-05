using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;

namespace AdminManagementDSL.AdminDSL.Common.Interfaces
{
    public interface INodeVisitor
    {
        Task Visit(AST node, List<SqlTableDto> sqlTablesDto);
    }
}
