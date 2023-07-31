using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class InvalidPropertyValueException : AdminDSLException
    {
        public InvalidPropertyValueException() : this(Error.ErrorCodes.Cd_InvalidPropertyValue) { }
        public InvalidPropertyValueException(string message) : base(message) { }
        public InvalidPropertyValueException(string message, Exception inner) : base(message, inner) { }
        public InvalidPropertyValueException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
