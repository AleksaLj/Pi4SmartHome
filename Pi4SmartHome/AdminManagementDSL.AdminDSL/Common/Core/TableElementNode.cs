using AdminManagementDSL.AdminDSL.Common.Interfaces;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class TableElementNode : AST
    {
        public Token TableName { get; set; }
        public Token TableType { get; set; }
        public List<AST> PropertyNodes { get; set; }

        public TableElementNode(Token tableName, Token tableType)
        {
            TableName = tableName;
            TableType = tableType;
            PropertyNodes = new List<AST>();
        }

        public override DataTable Accept(INodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
