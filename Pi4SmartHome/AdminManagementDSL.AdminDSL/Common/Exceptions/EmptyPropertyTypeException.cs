using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class EmptyPropertyTypeException : AdminDSLException
    {
        public EmptyPropertyTypeException() : this(Error.ErrorCodes.Cd_EmptyPropertyType) { }
        public EmptyPropertyTypeException(string message) : base(message) { }
        public EmptyPropertyTypeException(string message, Exception inner) : base(message, inner) { }
        public EmptyPropertyTypeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
