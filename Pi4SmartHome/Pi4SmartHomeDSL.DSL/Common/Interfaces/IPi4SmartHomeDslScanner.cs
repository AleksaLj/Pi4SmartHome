using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface IAdminDSLScanner
    {
        Task Configure(string programCode);
        Token? CurrentToken { get; set; }
        Task<Token?> GetNextToken();
    }
}
