using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class NullTableKeywordTokenException : AdminDSLException
    {
        public NullTableKeywordTokenException() : this(Error.ErrorCodes.Cd_NullTableKeywordToken) { }
        public NullTableKeywordTokenException(string message) : base(message) { }
        public NullTableKeywordTokenException(string message, Exception inner) : base(message, inner) { }
        public NullTableKeywordTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }

    }
}
