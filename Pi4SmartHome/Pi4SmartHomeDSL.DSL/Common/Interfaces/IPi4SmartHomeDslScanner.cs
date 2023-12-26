using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Interfaces
{
    public interface IPi4SmartHomeDslScanner
    {
        Task Configure(string programCode);
        Token? CurrentToken { get; set; }
        Task<Token?> GetNextToken();
    }
}
