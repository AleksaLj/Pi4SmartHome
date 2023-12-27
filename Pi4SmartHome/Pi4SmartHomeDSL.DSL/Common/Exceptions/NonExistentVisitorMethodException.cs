using System.Runtime.Serialization;

namespace Pi4SmartHomeDSL.DSL.Common.Exceptions
{
    public class NonExistentVisitorMethodException : Pi4SmartHomeDslException
    {
        public NonExistentVisitorMethodException() : this(Error.ErrorCodes.Cd_NonExistentNodeVisitor) { }
        public NonExistentVisitorMethodException(string message) : base(message) { }
        public NonExistentVisitorMethodException(string message, Exception inner) : base(message, inner) { }
        public NonExistentVisitorMethodException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
