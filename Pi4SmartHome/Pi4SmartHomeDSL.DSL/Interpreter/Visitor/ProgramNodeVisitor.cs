using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Core;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Interpreter.Visitor
{
    public class ProgramNodeVisitor : INodeVisitor
    {
        private readonly INodeVisitor _visitor;

        public ProgramNodeVisitor(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task<IoTDeviceMessage> Visit(AST node)
        {
            var messageStructureNode = (ProgramNode)node;

            return await _visitor.Visit(messageStructureNode.MessageStructure);
        }
    }
}
