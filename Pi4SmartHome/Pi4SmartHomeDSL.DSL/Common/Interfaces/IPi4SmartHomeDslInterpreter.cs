using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface IAdminDSLInterpreter
    {
        Task Interpret(AST tree);
    }
}
