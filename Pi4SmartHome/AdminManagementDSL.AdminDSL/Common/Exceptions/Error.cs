
namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public static class Error
    {
        public static class ErrorCodes
        {
            public const string Cd_InvalidToken = "Invalid Token!";
            public const string Cd_InvalidSyntax = "Invalid Syntax!";
            public const string Cd_InvalidIdentifierToken = "Unkown Identifier Token!";
        }

        public static class ErrorMessages
        {
            public static AdminDSLException InvalidTokenErr() => new InvalidTokenException();
            public static AdminDSLException InvalidSyntaxErr() => new InvalidSyntaxException();
            public static AdminDSLException UnkownIdentifierTokenErr() => new UnkownIdentifierTokenException();
        }
    }
}
