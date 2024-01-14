using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Common.Core
{
    public class MessageStructureNode : AST
    {
        public Token To { get; set; }
        public Token MessageBody { get; set; }
        public IEnumerable<AST> MessageProperties { get; set; }

        public MessageStructureNode(Token to, Token messageBody, IEnumerable<AST> messageProperties)
        {
            To = to;
            MessageBody = messageBody;
            MessageProperties = messageProperties;
        }

        public override async Task<IoTDeviceMessage> Accept(INodeVisitor visitor) => await visitor.Visit(this);
    }
}
