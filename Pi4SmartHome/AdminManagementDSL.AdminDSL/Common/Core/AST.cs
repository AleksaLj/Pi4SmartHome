using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public abstract class AST
    {
        public abstract Task Accept(INodeVisitor visitor, List<SqlTableDto> sqlTablesDto);
    }
}
