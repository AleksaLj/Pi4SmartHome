using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Core;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Interpreter
{
    public class Pi4SmartHomeDslInterpreter : IPi4SmartHomeDslInterpreter
    {
        private readonly INodeVisitor _visitor;

        public Pi4SmartHomeDslInterpreter(INodeVisitor visitor)
        {
            _visitor = visitor;
        }

        public async Task<IoTDeviceMessage> Interpret(AST tree)
        {
            var programNode = (ProgramNode)tree;

            var iotDeviceMessage = await _visitor.Visit(programNode);

            return iotDeviceMessage;
        }
    }
}
