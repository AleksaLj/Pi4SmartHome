using Pi4SmartHome.Core.Helper;
using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Core;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Interpreter.Visitor
{
    public class MessageStructureNodeVisitor : INodeVisitor
    {
        private readonly INodeVisitor _visitor;

        public MessageStructureNodeVisitor(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task<IoTDeviceMessage> Visit(AST node)
        {
            var messageStructureNode = (MessageStructureNode)node;

            var to = messageStructureNode.To;
            var messageBody = messageStructureNode.MessageBody;
            var messageProperties = messageStructureNode.MessageProperties;

            var messagePropertiesDict = new Dictionary<string, string>();
            foreach (var messagePropNode in messageProperties)
            {
                var messageProp = (MessagePropertyNode)messagePropNode;

                var messagePropName = messageProp.PropertyName.TokenValue?.ToString()!;
                var messagePropValue = messageProp.PropertyValue.TokenValue?.ToString()!;

                messagePropertiesDict.Add(messagePropName, messagePropValue);
            }

            var iotDeviceMessage = new IoTDeviceMessage(to.TokenValue?.ToString()!,
                                                        messageBody.TokenValue?.ToString(),
                                                        messagePropertiesDict);

            return (await TaskCache.ObjectValue(iotDeviceMessage))!;
        }
    }
}
