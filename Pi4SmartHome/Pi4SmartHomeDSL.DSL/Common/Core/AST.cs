using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Common.Core
{
    public abstract class AST
    {
        public abstract Task<IoTDeviceMessage> Accept(INodeVisitor visitor);
    }
}
