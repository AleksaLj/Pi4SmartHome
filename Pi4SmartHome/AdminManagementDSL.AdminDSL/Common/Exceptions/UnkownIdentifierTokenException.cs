using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class UnkownIdentifierTokenException : AdminDSLException
    {
        public UnkownIdentifierTokenException() : this(Error.ErrorCodes.Cd_InvalidIdentifierToken) { }
        public UnkownIdentifierTokenException(string message) : base(message) { }
        public UnkownIdentifierTokenException(string message, Exception inner) : base(message, inner) { }
        public UnkownIdentifierTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
