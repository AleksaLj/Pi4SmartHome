using DeviceManagement.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface INodeVisitor
    {
        Task<IoTDeviceMessage> Visit(AST node);
    }
}
