using Pi4SmartHomeDSL.Application.Models;
using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface IPi4SmartHomeDslInterpreter
    {
        Task<IoTDeviceMessage> Interpret(AST tree);
    }
}
