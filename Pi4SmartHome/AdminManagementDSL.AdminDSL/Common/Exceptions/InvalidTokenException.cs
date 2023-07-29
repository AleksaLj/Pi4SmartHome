using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class InvalidTokenException : AdminDSLException
    {
        public InvalidTokenException() : this(Error.ErrorCodes.Cd_InvalidToken) { }
        public InvalidTokenException(string message) : base(message) { }
        public InvalidTokenException(string message, Exception inner) : base(message, inner) { }
        public InvalidTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
