using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class NullTableTypeKeywordTokenException : AdminDSLException
    {
        public NullTableTypeKeywordTokenException() : this(Error.ErrorCodes.Cd_NullTableTypeKeywordToken) { }
        public NullTableTypeKeywordTokenException(string message) : base(message) { }
        public NullTableTypeKeywordTokenException(string message, Exception inner) : base(message, inner) { }
        public NullTableTypeKeywordTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
