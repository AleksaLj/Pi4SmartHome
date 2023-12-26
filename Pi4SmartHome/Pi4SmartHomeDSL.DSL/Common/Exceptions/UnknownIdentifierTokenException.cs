using System.Runtime.Serialization;

namespace Pi4SmartHomeDSL.DSL.Common.Exceptions
{
    public class UnknownIdentifierTokenException : Pi4SmartHomeDslException
    {
        public UnknownIdentifierTokenException() : this(Error.ErrorCodes.Cd_UnknownIdentifierToken) { }
        public UnknownIdentifierTokenException(string message) : base(message) { }
        public UnknownIdentifierTokenException(string message, Exception inner) : base(message, inner) { }
        public UnknownIdentifierTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
