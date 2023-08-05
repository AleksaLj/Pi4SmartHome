using AdminManagementDSL.AdminDSL.Common.Dto;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

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

        public override async Task Accept(INodeVisitor visitor, 
                                          List<SqlTableDto> sqlTablesDto) => await visitor.Visit(this, sqlTablesDto);
    }
}
