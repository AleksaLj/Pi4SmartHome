using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using Pi4SmartHome.Core.Helper;

namespace AdminManagementDSL.AdminDSL.Interpreter.Visitor
{
    public class TableElementNodeVisitor : INodeVisitor
    {
        private readonly INodeVisitor _visitor;

        public TableElementNodeVisitor(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task Visit(AST node, List<SqlTableDto> sqlTablesDto)
        {
            var tableElementNode = (TableElementNode)node;
            var sqlTableDto = new SqlTableDto();

            var tableName = tableElementNode.TableName.TokenValue?.ToString();
            var tableType = tableElementNode.TableType.TokenValue?.ToString();
            sqlTableDto.TableName = tableName!;
            sqlTableDto.TableType = tableType!;

            foreach (var property in tableElementNode.PropertyNodes)
            { 
                var propertyNode = (PropertyNode)property;
                var sqlColumn = new SqlColumnDto(propertyNode.PropertyName.TokenValue?.ToString()!);
                var sqlRow = new SqlRowDto(sqlColumn, propertyNode.PropertyValue.TokenValue);
                sqlTableDto.Rows.Add(sqlRow);
            }
            sqlTablesDto.Add(sqlTableDto);

            await TaskCache.True;
        }
    }
}
