using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class InvalidSyntaxException : AdminDSLException
    {
        public InvalidSyntaxException() : this(Error.ErrorCodes.Cd_InvalidSyntax) { }
        public InvalidSyntaxException(string message) : base(message) { }
        public InvalidSyntaxException(string message, Exception inner) : base(message, inner) { }
        public InvalidSyntaxException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
