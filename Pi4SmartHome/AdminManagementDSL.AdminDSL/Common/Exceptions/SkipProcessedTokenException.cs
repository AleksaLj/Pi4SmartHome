using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class SkipProcessedTokenException : AdminDSLException
    {
        public SkipProcessedTokenException() : this(Error.ErrorCodes.Cd_SkipProcessedToken) { }
        public SkipProcessedTokenException(string message) : base(message) { }
        public SkipProcessedTokenException(string message, Exception inner) : base(message, inner) { }
        public SkipProcessedTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
