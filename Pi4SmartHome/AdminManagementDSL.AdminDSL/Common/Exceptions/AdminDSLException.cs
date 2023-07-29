using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public class AdminDSLException : Exception
    {
        public AdminDSLException() { }
        public AdminDSLException(string message) : base(message) { }
        public AdminDSLException(string message, Exception inner) : base(message, inner) { }
        protected AdminDSLException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
