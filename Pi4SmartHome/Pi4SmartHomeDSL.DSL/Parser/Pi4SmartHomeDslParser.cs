using Pi4SmartHome.Core.Helper;
using Pi4SmartHomeDSL.DSL.Common.Core;
using Pi4SmartHomeDSL.DSL.Common.Exceptions;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

namespace Pi4SmartHomeDSL.DSL.Parser
{
    public class Pi4SmartHomeDslParser : IPi4SmartHomeDslParser
    {
        private IPi4SmartHomeDslScanner? _scanner;

        private async void SkipProcessedToken(TokenTypeEnum tokenType, string? reservedKeyword = null)
        {
            if (_scanner?.CurrentToken?.TokenType == tokenType &&
                (string.IsNullOrEmpty(reservedKeyword) || _scanner?.CurrentToken?.TokenValue?.ToString() == reservedKeyword))
            {
                _scanner.CurrentToken = await _scanner.GetNextToken();
            }
            else
            {
                throw Error.ErrorMessages.SkipProcessedTokenErr();
            }
        }

        //property_assignment: property_name ASSIGN property_value
        private AST MessagePropertyEl()
        {
            var propertyName = _scanner?.CurrentToken ?? throw Error.ErrorMessages.PropertyNullException();

            SkipProcessedToken(TokenTypeEnum.PROPERTY_NAME);
            SkipProcessedToken(TokenTypeEnum.ASSIGN);

            var propertyValue = (_scanner?.CurrentToken) ?? throw Error.ErrorMessages.PropertyNullException();

            SkipProcessedToken(TokenTypeEnum.PROPERTY_VAL);

            return new MessagePropertyNode(propertyName, propertyValue);
        }

        //message_props: property_assignment | property_assignment COMMA
        private IEnumerable<AST> MessagePropsEl()
        {
            var messageProperties = new List<AST>();

            var firstMessageProperty = MessagePropertyEl();
            messageProperties.Add(firstMessageProperty);

            while (_scanner?.CurrentToken?.TokenType == TokenTypeEnum.COMMA)
            {
                SkipProcessedToken(TokenTypeEnum.COMMA);
                var messageProperty = MessagePropertyEl();
                messageProperties.Add(messageProperty);
            }

            return messageProperties;
        }

        //message_properties_el: MessageProperties COLON BEGIN message_props END
        private IEnumerable<AST> MessagePropertiesEl()
        {
            SkipProcessedToken(TokenTypeEnum.MessagePropertyKeyword, ReservedKeywords.Pi4SmartHomeDslPropertyKeywords.Keyword_MessageProperties);
            SkipProcessedToken(TokenTypeEnum.COLON);
            SkipProcessedToken(TokenTypeEnum.Pi4SmartHomeDslKeyword, ReservedKeywords.Pi4SmartHomeDslKeywords.Keyword_Begin);

            var messageProperties = MessagePropsEl();

            SkipProcessedToken(TokenTypeEnum.Pi4SmartHomeDslKeyword, ReservedKeywords.Pi4SmartHomeDslKeywords.Keyword_End);

            return messageProperties;
        }

        //message_body_el: MessageBody COLON prop_val
        private Token MessageBodyEl()
        {
            SkipProcessedToken(TokenTypeEnum.MessagePropertyKeyword, ReservedKeywords.Pi4SmartHomeDslPropertyKeywords.Keyword_MessageBody);
            SkipProcessedToken(TokenTypeEnum.COLON);

            var messageBodyPropValue = _scanner?.CurrentToken;

            SkipProcessedToken(TokenTypeEnum.PROPERTY_VAL);

            return new Token(TokenTypeEnum.MessagePropertyKeyword, messageBodyPropValue?.TokenValue);
        }

        //to_el: To COLON prop_val
        private Token ToEl()
        {
            SkipProcessedToken(TokenTypeEnum.MessagePropertyKeyword, ReservedKeywords.Pi4SmartHomeDslPropertyKeywords.Keyword_To);
            SkipProcessedToken(TokenTypeEnum.COLON);

            var toPropValue = _scanner?.CurrentToken;

            SkipProcessedToken(TokenTypeEnum.PROPERTY_VAL);

            return new Token(TokenTypeEnum.MessagePropertyKeyword, toPropValue?.TokenValue);
        }

        //message_structure: to_el message_body_el message_properties_el
        private AST MessageStructure()
        {
            var to = ToEl();
            var messageBody = MessageBodyEl();
            var messageProperties = MessagePropertiesEl();

            return new MessageStructureNode(to, messageBody, messageProperties);
        }

        //program: SEND_DEVICE_MESSAGE message_structure
        private AST ProgramRoot()
        {
            SkipProcessedToken(TokenTypeEnum.Pi4SmartHomeDslKeyword, ReservedKeywords.Pi4SmartHomeDslKeywords.Keyword_SendDeviceMsg);

            var messageStructureNode = MessageStructure();
            var programNode = new ProgramNode(messageStructureNode);

            return programNode;
        }
        
        public async Task<AST> Parse(IPi4SmartHomeDslScanner scanner)
        {
            _scanner = scanner;
            _scanner.CurrentToken = await _scanner.GetNextToken();

            var root = ProgramRoot();

            if (_scanner.CurrentToken?.TokenType != TokenTypeEnum.EOF)
            {
                throw Error.ErrorMessages.InvalidSyntaxErr();
            }

            return (await TaskCache.ObjectValue(root))!;
        }
    }
}
