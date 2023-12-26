using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface IPi4SmartHomeDslParser
    {
        Task<AST> Parse(IPi4SmartHomeDslScanner scanner);
    }
}
