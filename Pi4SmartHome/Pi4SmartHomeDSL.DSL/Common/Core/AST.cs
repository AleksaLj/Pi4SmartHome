using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Common.Core
{
    public abstract class AST
    {
        public abstract Task Accept(INodeVisitor visitor);
    }
}
