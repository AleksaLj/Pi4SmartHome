using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class EmptyPropertyNameException : AdminDSLException
    {
        public EmptyPropertyNameException() : this(Error.ErrorCodes.Cd_EmptyPropertyName) { }
        public EmptyPropertyNameException(string message) : base(message) { }
        public EmptyPropertyNameException(string message, Exception inner) : base(message, inner) { }
        public EmptyPropertyNameException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
