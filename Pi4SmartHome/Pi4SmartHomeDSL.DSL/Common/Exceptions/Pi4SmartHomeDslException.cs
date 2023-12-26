
using System.Runtime.Serialization;

namespace Pi4SmartHomeDSL.DSL.Common.Exceptions
{
    public class Pi4SmartHomeDslException : Exception
    {
        public Pi4SmartHomeDslException() { }
        public Pi4SmartHomeDslException(string message) : base(message) { }
        public Pi4SmartHomeDslException(string message, Exception inner) : base(message, inner) { }
        public Pi4SmartHomeDslException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
        
    }
}
