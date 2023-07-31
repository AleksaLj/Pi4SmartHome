
namespace AdminManagementDSL.AdminDSL.Common.Exceptions
{
    public static class Error
    {
        public static class ErrorCodes
        {
            public const string Cd_InvalidToken = "Invalid Token!";
            public const string Cd_InvalidSyntax = "Invalid Syntax!";
            public const string Cd_UnkownIdentifierToken = "Unkown Identifier Token!";
            public const string Cd_SkipProcessedToken = "The provided token is not same as the current token!";
            public const string Cd_NullTableKeywordToken = "Null reference [Table keyword token]!";
            public const string Cd_NullTableTypeKeywordToken = "Null reference [Table type keyword token]!";
            public const string Cd_EmptyTableProperties = "Argument exception [Empty table properties error]!";
            public const string Cd_EmptyPropertyName = "Argument exception [Empty property name]!";
            public const string Cd_EmptyPropertyType = "Argument exception [Empty property type]!";
            public const string Cd_EmptyPropertyValue = "Argument exception [Empty property value]!";
            public const string Cd_InvalidPropertyValue = "Invalid property value!";
        }

        public static class ErrorMessages
        {
            public static AdminDSLException InvalidTokenErr() => new InvalidTokenException();
            public static AdminDSLException InvalidSyntaxErr() => new InvalidSyntaxException();
            public static AdminDSLException UnkownIdentifierTokenErr() => new UnkownIdentifierTokenException();
            public static AdminDSLException SkipProcessedTokenErr() => new SkipProcessedTokenException();
            public static AdminDSLException NullTableKeywordTokenErr() => new NullTableKeywordTokenException();
            public static AdminDSLException NullTableTypeKeywordTokenErr() => new NullTableTypeKeywordTokenException();
            public static AdminDSLException EmptyTablePropertiesErr() => new EmptyTablePropertiesException();
            public static AdminDSLException EmptyPropertyNameErr() => new EmptyPropertyNameException();
            public static AdminDSLException EmptyPropertyTypeErr() => new EmptyPropertyTypeException();
            public static AdminDSLException EmptyPropertyValueErr() => new EmptyPropertyValueException();
            public static AdminDSLException InvalidPropertyValueErr() => new InvalidPropertyValueException();
        }
    }
}
