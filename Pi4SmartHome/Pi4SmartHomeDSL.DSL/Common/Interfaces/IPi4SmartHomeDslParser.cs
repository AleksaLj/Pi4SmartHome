using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface IAdminDSLParser
    {
        Task<AST> Parse(IAdminDSLScanner scanner);
    }
}
