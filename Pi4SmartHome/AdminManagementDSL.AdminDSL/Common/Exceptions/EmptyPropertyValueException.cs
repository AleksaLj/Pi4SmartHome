using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class EmptyPropertyValueException : AdminDSLException
    {
        public EmptyPropertyValueException() : this(Error.ErrorCodes.Cd_EmptyPropertyValue) { }
        public EmptyPropertyValueException(string message) : base(message) { }
        public EmptyPropertyValueException(string message, Exception inner) : base(message, inner) { }
        public EmptyPropertyValueException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
