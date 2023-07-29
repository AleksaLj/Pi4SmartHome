using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Interfaces;

namespace AdminManagementDSL.AdminDSL.Parser
{
    public class AdminDSLParser : IAdminDSLParser
    {
        public Task<AST> Parse(IAdminDSLScanner scanner)
        {
            throw new NotImplementedException();
        }
    }
}
