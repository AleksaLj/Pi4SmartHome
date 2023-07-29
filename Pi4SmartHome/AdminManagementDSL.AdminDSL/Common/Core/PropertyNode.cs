using AdminManagementDSL.AdminDSL.Common.Interfaces;
using System.Data;

namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public class PropertyNode : AST
    {
        public Token PropertyName { get; set; }
        public Token PropertyType { get; set; }
        public Token PropertyValue { get; set; }

        public PropertyNode(Token propertyName, Token propertyType, Token propertyValue)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            PropertyValue = propertyValue;
        }

        public override DataTable Accept(INodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
