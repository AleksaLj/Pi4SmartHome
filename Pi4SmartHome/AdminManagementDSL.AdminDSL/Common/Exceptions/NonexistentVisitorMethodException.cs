using System.Runtime.Serialization;

namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    [Serializable]
    public class NonexistentVisitorMethodException : AdminDSLException
    {
        public NonexistentVisitorMethodException() : this(Error.ErrorCodes.Cd_NonexistentVisitMethod) { }
        public NonexistentVisitorMethodException(string message) : base(message) { }
        public NonexistentVisitorMethodException(string message, Exception inner) : base(message, inner) { }
        protected NonexistentVisitorMethodException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
