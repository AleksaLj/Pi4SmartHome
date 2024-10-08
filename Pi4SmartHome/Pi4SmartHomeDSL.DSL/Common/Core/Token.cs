﻿
namespace Pi4SmartHomeDSL.DSL.Common.Core
{
    public class Token
    {
        public TokenTypeEnum TokenType { get; set; }
        public object? TokenValue { get; set; }

        public Token(TokenTypeEnum tokenType, object? tokenValue)
        {
            TokenType = tokenType;
            TokenValue = tokenValue;
        }

        public override string ToString()
        {
            return $"{TokenValue}";
        }
    }
}
