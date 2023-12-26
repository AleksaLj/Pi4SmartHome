using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface IPi4SmartHomeDslInterpreter
    {
        Task Interpret(AST tree);
    }
}
