using System.Runtime.Serialization;

namespace Pi4SmartHomeDSL.DSL.Common.Exceptions
{
    public class PropertyNullException : Pi4SmartHomeDslException
    {
        public PropertyNullException() : this(Error.ErrorCodes.Cd_PropertyNull) { }
        public PropertyNullException(string message) : base(message) { }
        public PropertyNullException(string message, Exception inner) : base(message, inner) { }
        public PropertyNullException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
