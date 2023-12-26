
using System.Runtime.Serialization;

namespace Pi4SmartHomeDSL.DSL.Common.Exceptions
{
    public class InvalidSyntaxException : Pi4SmartHomeDslException
    {
        public InvalidSyntaxException() : this(Error.ErrorCodes.Cd_InvalidSyntax) { }
        public InvalidSyntaxException(string message) : base(message) { }
        public InvalidSyntaxException(string message, Exception inner) : base(message, inner) { }
        public InvalidSyntaxException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
