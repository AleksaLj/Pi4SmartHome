using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class TableElementNode : AST
    {
        public Token TableName { get; set; }
        public Token TableType { get; set; }
        public IEnumerable<AST> PropertyNodes { get; set; }

        public TableElementNode(Token tableName, Token tableType)
        {
            TableName = tableName;
            TableType = tableType;
            PropertyNodes = new List<AST>();
        }

        public TableElementNode(Token tableName, Token tableType, IEnumerable<AST> propertyNodes)
        {
            TableName = tableName;
            TableType = tableType;
            PropertyNodes = propertyNodes;
        }

        public override async Task Accept(INodeVisitor visitor, 
                                          List<SqlTableDto> sqlTablesDto) => await visitor.Visit(this, sqlTablesDto);
    }
}
