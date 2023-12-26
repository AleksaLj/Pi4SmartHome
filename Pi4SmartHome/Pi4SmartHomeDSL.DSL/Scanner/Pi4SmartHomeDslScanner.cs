using Pi4SmartHome.Core.Helper;
using Pi4SmartHomeDSL.DSL.Common.Core;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;
using System.Text;
using Pi4SmartHomeDSL.DSL.Common.Exceptions;
using Pi4SmartHomeDSL.DSL.Common.Extensions;

namespace Pi4SmartHomeDSL.DSL.Scanner
{
    public class Pi4SmartHomeDslScanner : IPi4SmartHomeDslScanner
    {
        public string ProgramCode { get; set; } = string.Empty;
        public Token? CurrentToken { get; set; }
        public int Position { get; set; }
        public char CurrentChar { get; set; }

        public Task Configure(string programCode)
        {
            ProgramCode = programCode.Trim();
            CurrentToken = null;
            Position = 0;
            CurrentChar = ProgramCode[Position];

            return TaskCache.True;
        }

        private void PositionUp(int positionUpLength = 1)
        {
            Position += positionUpLength;

            CurrentChar = Position > ProgramCode.Length - 1 ? default : ProgramCode[Position];
        }

        private Task SkipWhiteSpaces()
        {
            while (CurrentChar is ' ' or '\r' or '\n' or '\t')
            {
                PositionUp();
            }

            return TaskCache.True;
        }

        private bool ShouldContinueReadingPropertyValue()
        {
            return CurrentChar != '`';
        }

        private string ReadPropertyValue()
        {
            StringBuilder propertyValue = new();

            while (ShouldContinueReadingPropertyValue())
            {
                propertyValue.Append(CurrentChar);
                PositionUp();
            }
            PositionUp();

            return propertyValue.ToString();
        }

        private bool ShouldContinueReadingIdentifierValue()
        {
            return CurrentChar != default(char)
                   && CurrentChar != '\r'
                   && CurrentChar != '\n'
                   && CurrentChar != '\t'
                   && (char.IsLetterOrDigit(CurrentChar) || CurrentChar == '-' || CurrentChar == '_');
        }

        private string ReadIdentifierValue()
        {
            var identifierValue = new StringBuilder();

            while (ShouldContinueReadingIdentifierValue())
            {
                identifierValue.Append(CurrentChar);
                PositionUp();
            }

            return identifierValue.ToString();
        }

        private Token ReadIdentifierToken()
        {
            var identifierValue = ReadIdentifierValue();

            if (identifierValue.IsIdentifierValuePi4SmartHomeKeyword())
            {
                return new Token(TokenTypeEnum.Pi4SmartHomeDslKeyword, identifierValue);
            }

            if (identifierValue.IsIdentifierValuePropertyKeyword())
            {
                return new Token(TokenTypeEnum.MessagePropertyKeyword, identifierValue);
            }

            return new Token(TokenTypeEnum.PROPERTY_NAME, identifierValue);
        }

        public async Task<Token?> GetNextToken()
        {
            while (CurrentChar != default(char))
            {
                await SkipWhiteSpaces();

                if (CurrentChar == '`')
                {
                    PositionUp();
                    var propertyValue = ReadPropertyValue();
                    var token = new Token(TokenTypeEnum.PROPERTY_VAL, propertyValue);

                    return await TaskCache.ObjectValue(token);
                }

                if (CurrentChar == ':')
                {
                    var token = new Token(TokenTypeEnum.COLON, CurrentChar);
                    PositionUp();

                    return await TaskCache.ObjectValue(token);
                }

                if (CurrentChar == '=')
                {
                    var token = new Token(TokenTypeEnum.ASSIGN, CurrentChar);
                    PositionUp();

                    return await TaskCache.ObjectValue(token);
                }

                if (CurrentChar == ',')
                {
                    var token = new Token(TokenTypeEnum.COMMA, CurrentChar);
                    PositionUp();

                    return await TaskCache.ObjectValue(token);
                }

                if (char.IsLetterOrDigit(CurrentChar))
                {
                    var token = ReadIdentifierToken();

                    return await TaskCache.ObjectValue(token);
                }

                throw Error.ErrorMessages.InvalidTokenErr();
            }

            var eofToken = new Token(TokenTypeEnum.EOF, null);

            return await TaskCache.ObjectValue(eofToken);
        }
    }
}
