using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Core;
using Pi4SmartHomeDSL.DSL.Common.Exceptions;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Interpreter.Visitor
{
    public class NodeVisitor : INodeVisitor
    {
        public async Task<IoTDeviceMessage> Visit(AST node)
        {
            return node switch
            {
                ProgramNode => await node.Accept(new ProgramNodeVisitor(this)),
                MessageStructureNode => await node.Accept(new MessageStructureNodeVisitor(this)),
                _ => throw Error.ErrorMessages.NonExistentVisitorMethodException()
            };
        }
    }
}
