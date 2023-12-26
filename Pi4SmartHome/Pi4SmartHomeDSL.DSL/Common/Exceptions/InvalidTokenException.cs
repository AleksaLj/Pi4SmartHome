using System.Runtime.Serialization;

namespace Pi4SmartHomeDSL.DSL.Common.Exceptions
{
    public class InvalidTokenException : Pi4SmartHomeDslException
    {
        public InvalidTokenException() : this(Error.ErrorCodes.Cd_InvalidToken) { }
        public InvalidTokenException(string message) : base(message) { }
        public InvalidTokenException(string message, Exception inner) : base(message, inner) { }
        public InvalidTokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
