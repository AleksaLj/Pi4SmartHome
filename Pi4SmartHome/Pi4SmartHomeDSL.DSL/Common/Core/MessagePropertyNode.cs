using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Common.Core
{
    public class MessagePropertyNode : AST
    {
        public Token PropertyName { get; set; }
        public Token PropertyValue { get; set; }

        public MessagePropertyNode(Token propertyName, Token propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public override async Task<IoTDeviceMessage> Accept(INodeVisitor visitor) => await visitor.Visit(this);
    }
}
