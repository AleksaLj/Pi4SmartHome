
namespace Pi4SmartHomeDSL.DSL.Common.Exceptions
{
    public static class Error
    {
        public static class ErrorCodes
        {
            public const string Cd_InvalidToken = "Invalid Token!";
            public const string Cd_InvalidSyntax = "Invalid Syntax!";
            public const string Cd_SkipProcessedToken = "The provided token is not same as the current token!";
            public const string Cd_UnknownIdentifierToken = "Unkown Identifier Token!";
            public const string Cd_PropertyNull = "Property (name or value) cannot be null!";
            public const string Cd_NonExistentNodeVisitor = "Non existent visitor method!";
        }

        public static class ErrorMessages
        {
            public static Pi4SmartHomeDslException InvalidTokenErr() => new InvalidTokenException();
            public static Pi4SmartHomeDslException InvalidSyntaxErr() => new InvalidSyntaxException();
            public static Pi4SmartHomeDslException SkipProcessedTokenErr() => new SkipProcessedTokenException();
            public static Pi4SmartHomeDslException UnknownIdentifierTokenErr() => new UnknownIdentifierTokenException();
            public static Pi4SmartHomeDslException PropertyNullException() => new PropertyNullException();
            public static Pi4SmartHomeDslException NonExistentVisitorMethodException() => new NonExistentVisitorMethodException();
        }
    }
}
