using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Exceptions;
using AdminManagementDSL.AdminDSL.Common.Extensions;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using Pi4SmartHome.Core.Helper;
using System.Text;

namespace AdminManagementDSL.AdminDSL.Scanner
{
    public class AdminDSLScanner : IAdminDSLScanner
    {
        public string ProgramCode { get; set; } = string.Empty;
        public Token? CurrentToken { get; set; }
        public int Position { get; set; }
        public char CurrentChar { get; set; }

        public AdminDSLScanner() { }


        public Task Configure(string programCode)
        {
            ProgramCode = programCode;
            CurrentToken = null;
            Position = 0;
            CurrentChar = ProgramCode[Position];

            return TaskCache.True;
        }

        private void PositionUp(int positionUpLength = 1)
        {
            Position += positionUpLength;

            if (Position > ProgramCode.Length - 1)
                CurrentChar = default;
            else
                CurrentChar = ProgramCode[Position];
        }

        private char Peek(int peekNumber)
        {
            int peekPosition = Position + peekNumber;

            if (peekPosition > ProgramCode.Length - 1)
                return default;

            return ProgramCode[peekPosition];
        }

        private Task SkipWhiteSpaces()
        {
            while (CurrentChar == ' ' 
                || CurrentChar == '\r' 
                || CurrentChar == '\n'
                || CurrentChar == '\t')
            {
                PositionUp();
            }

            return TaskCache.True;
        }

        private bool ShouldContinueReadingPropertyValue()
        {
            if (CurrentChar != '`'
                || (CurrentChar == '`' 
                        && Peek(1) != default(char) 
                        && Peek(1) != ' ' 
                        && Peek(1) != '\r' 
                        && Peek(1) != '\n'
                        && Peek(1) != '\t')
               )
            {
                return true;
            }

            return false;
        }        

        private string ReadPropertyValue()
        {
            StringBuilder propertyValue = new StringBuilder();

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
            if (CurrentChar != default(char)
                && CurrentChar != '\r'
                && CurrentChar != '\n'
                && CurrentChar != '\t'
                && (Char.IsLetterOrDigit(CurrentChar) || CurrentChar == '.' || CurrentChar == '_'))
            {
                return true;
            }

            return false;
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
            string identifierValue = ReadIdentifierValue();

            if (identifierValue.IsIdentifierValueAdminDSLKeyword())
            {
                return new Token(TokenTypeEnum.AdminDSLKeyword, identifierValue);
            }

            if (identifierValue.IsIdentifierValuePropertyKeyword())
            {
                return new Token(TokenTypeEnum.PropertyKeyword, identifierValue);
            }

            if (identifierValue.IsIdentifierValueTableKeyword())
            {
                return new Token(TokenTypeEnum.TableKeyword, identifierValue);
            }

            if (identifierValue.IsIdentifierValueTypeKeyword())
            {
                return new Token(TokenTypeEnum.TypeKeyword, identifierValue);
            }

            if (identifierValue == ReservedKeywords.LogicalOperatorKeywords.Keyword_And)
            {
                return new Token(TokenTypeEnum.AND, identifierValue);
            }

            throw Error.ErrorMessages.UnkownIdentifierTokenErr();
        }

        public async Task<Token> GetNextToken()
        {
            while (CurrentChar != default(char))
            {
                var result = SkipWhiteSpaces();

                if (CurrentChar == '`')
                {
                    PositionUp();
                    var propertyValue = ReadPropertyValue();
                    var token = new Token(TokenTypeEnum.PROPERTY, propertyValue);

                    return await TaskCache.ObjectValue<Token>(token);
                }

                if (CurrentChar == ':')
                {
                    var token = new Token(TokenTypeEnum.COLON, CurrentChar);
                    PositionUp();

                    return await TaskCache.ObjectValue<Token>(token);
                }

                if(CurrentChar == '=')
                {
                    var token = new Token(TokenTypeEnum.ASSIGN, '=');
                    PositionUp();

                    return await TaskCache.ObjectValue<Token>(token);
                }

                if (CurrentChar == ';')
                {
                    var token = new Token(TokenTypeEnum.SEMI, ';');
                    PositionUp();

                    return await TaskCache.ObjectValue<Token>(token);
                }

                if(CurrentChar == '{')
                {
                    var token = new Token(TokenTypeEnum.LCurlyBracket, '{');
                    PositionUp();

                    return await TaskCache.ObjectValue<Token>(token);
                }

                if (CurrentChar == '}')
                {
                    var token = new Token(TokenTypeEnum.RCurlyBracket, '}');
                    PositionUp();

                    return await TaskCache.ObjectValue<Token>(token);
                }

                if (Char.IsLetterOrDigit(CurrentChar))
                {
                    var token = ReadIdentifierToken();

                    return await TaskCache.ObjectValue<Token>(token);
                }

                throw Error.ErrorMessages.InvalidTokenErr();
            }

            var eofToken = new Token(TokenTypeEnum.EOF, null);
            return await TaskCache.ObjectValue<Token>(eofToken);
        }
    }
}
