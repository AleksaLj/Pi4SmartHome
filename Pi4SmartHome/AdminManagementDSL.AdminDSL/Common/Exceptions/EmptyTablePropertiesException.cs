using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class EmptyTablePropertiesException : AdminDSLException
    {
        public EmptyTablePropertiesException() : this(Error.ErrorCodes.Cd_EmptyTableProperties) { }
        public EmptyTablePropertiesException(string message) : base(message) { }
        public EmptyTablePropertiesException(string message, Exception inner) : base(message, inner) { }
        public EmptyTablePropertiesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
