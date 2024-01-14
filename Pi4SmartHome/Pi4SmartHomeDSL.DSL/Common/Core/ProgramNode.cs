using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Common.Core
{
    public class ProgramNode : AST
    {
        public AST MessageStructure { get; set; }

        public ProgramNode(AST messageStructure)
        {
            MessageStructure = messageStructure;
        }
        public override async Task<IoTDeviceMessage> Accept(INodeVisitor visitor) => await visitor.Visit(this);
    }
}
