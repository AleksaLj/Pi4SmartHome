using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Exceptions;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using Pi4SmartHome.Core.Helper;
using System.Text.RegularExpressions;

namespace AdminManagementDSL.AdminDSL.Parser
{
    public class AdminDSLParser : IAdminDSLParser
    {
        private IAdminDSLScanner? _scanner;
        
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

        //property_value: (a-zA-Z0-9\s)+ | (([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])) | NULL
        private static bool IsValidEmails(string? propertyValue = null)
        {
            var emailRegex = new Regex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");

            if (propertyValue == null)
            {
                return true;
            }

            var emails = propertyValue.Split(",");
            foreach (var email in emails)
            {
                var emailTrim = email.Trim();
                if (!emailRegex.IsMatch(emailTrim))
                    return false;
            }

            return true;
        }
        
        private static bool IsPropertyValueValid(string? propertyValue = null)
        {
            var propertyValueRegex = new Regex("^\\b(?:\\w|\\s|-|!)+\\b$");
            
            if (propertyValue == null)
            {
                return true;
            }

            var propertyValues = propertyValue.Split(",");
            foreach (var propValue in propertyValues)
            {
                var propValueTrim = propValue.Trim();
                if (!propertyValueRegex.IsMatch(propValueTrim))
                {
                    return false;
                }
            }

            return true;
        }

        //property_assignment: property_name COLON type_keywords ASSIGN 'property_value'
        private AST PropertyAssignment()
        {
            var propertyNameToken = _scanner?.CurrentToken;
            if (propertyNameToken == null || propertyNameToken.TokenValue == null)
            {
                throw Error.ErrorMessages.EmptyPropertyNameErr();
            }
            SkipProcessedToken(TokenTypeEnum.PropertyKeyword);

            SkipProcessedToken(TokenTypeEnum.COLON);
            var propertyTypeToken = _scanner?.CurrentToken;
            if (propertyTypeToken == null)
            { 
                throw Error.ErrorMessages.EmptyPropertyTypeErr();
            }
            SkipProcessedToken(TokenTypeEnum.TypeKeyword);

            SkipProcessedToken(TokenTypeEnum.ASSIGN);

            var propertyValueToken = _scanner?.CurrentToken;
            if(propertyValueToken == null)
            {
                throw Error.ErrorMessages.EmptyPropertyValueErr();
            }

            var isPropertyValueValid = propertyNameToken.TokenValue.ToString() == ReservedKeywords.PropertyKeywords.Keyword_Users ?
                IsValidEmails((string?)propertyValueToken.TokenValue) :
                IsPropertyValueValid((string?)propertyValueToken.TokenValue);

            if (!isPropertyValueValid)
            {
                throw Error.ErrorMessages.InvalidPropertyValueErr();
            }
            SkipProcessedToken(TokenTypeEnum.PROPERTY);

            var propertyNode = new PropertyNode(propertyNameToken, propertyTypeToken, propertyValueToken);

            return propertyNode;
        }

        //property_definition: property_assignment ( (AND) property_assignment)*
        private IEnumerable<AST> PropertyDefinition()
        {
            var propertyAssignmentNode = PropertyAssignment();

            yield return propertyAssignmentNode;

            while (_scanner?.CurrentToken?.TokenType == TokenTypeEnum.AND)
            {
                SkipProcessedToken(TokenTypeEnum.AND);
                var additionalPropertyAssignmentNode = PropertyAssignment();
                yield return additionalPropertyAssignmentNode;
            }
        }

        //table_properties: property_definition+
        private IEnumerable<AST> TableProperties()
        {
            var tablePropertiesNodes = new List<AST>();

            foreach (var tableProperty in PropertyDefinition())
            {
                tablePropertiesNodes.Add(tableProperty);
            }

            if (tablePropertiesNodes == null || tablePropertiesNodes.Count() == 0)
            {
                throw Error.ErrorMessages.EmptyTablePropertiesErr();
            }

            return tablePropertiesNodes;
        }

        //table_element: DEFINE table_keywords COLON type_keywords table_properties SEMI
        private AST TableElement()
        {
            SkipProcessedToken(TokenTypeEnum.AdminDSLKeyword, ReservedKeywords.AdminDSLKeywords.Keyword_Define);

            var tableKeywordToken = _scanner?.CurrentToken;
            if (tableKeywordToken == null)
            {
                throw Error.ErrorMessages.NullTableKeywordTokenErr();
            }
            SkipProcessedToken(TokenTypeEnum.TableKeyword);

            SkipProcessedToken(TokenTypeEnum.COLON);
            var tableTypeKeywordToken = _scanner?.CurrentToken;
            if (tableTypeKeywordToken == null || tableTypeKeywordToken.TokenValue?.ToString() != ReservedKeywords.TypeKeywords.Keyword_Table)
            {
                throw Error.ErrorMessages.NullTableTypeKeywordTokenErr();
            }
            SkipProcessedToken(TokenTypeEnum.TypeKeyword);

            var tableProperties = TableProperties();
            SkipProcessedToken(TokenTypeEnum.SEMI);

            var tableElementNode = new TableElementNode(tableKeywordToken, tableTypeKeywordToken, tableProperties);

            return tableElementNode;
        }

        //compound_element: table_element | table_element compound_element
        private AST CompoundElement()
        {
            var tableElementNode = TableElement();
            var tableElementNodes = new List<AST>
            { 
                tableElementNode
            };

            while (_scanner?.CurrentToken?.TokenType != TokenTypeEnum.RCurlyBracket)
            {
                var additionalTableElementNode = TableElement();
                tableElementNodes.Add(additionalTableElementNode);
            }
            var compoundElementNode = new CompoundElementNode(tableElementNodes);

            return compoundElementNode;
        }

        //provision_main: BEGIN lCurlyBracket compound_element rCurlyBracket END
        private AST ProvisionMain()
        {
            SkipProcessedToken(TokenTypeEnum.AdminDSLKeyword, ReservedKeywords.AdminDSLKeywords.Keyword_Begin);
            SkipProcessedToken(TokenTypeEnum.LCurlyBracket);

            var compoundElementNode = CompoundElement();
            var provisionMainNode = new ProvisionMainNode(compoundElementNode);

            SkipProcessedToken(TokenTypeEnum.RCurlyBracket);
            SkipProcessedToken(TokenTypeEnum.AdminDSLKeyword, ReservedKeywords.AdminDSLKeywords.Keyword_End);

            return provisionMainNode;
        }

        //program: PI4SMARTHOMEADMIN.PROVISION provision_main
        private AST ProgramRoot()
        {
            SkipProcessedToken(TokenTypeEnum.AdminDSLKeyword, ReservedKeywords.AdminDSLKeywords.Keyword_Provision);

            var provisionMainNode = ProvisionMain();
            var programNode = new ProgramNode(provisionMainNode);

            return programNode;
        }

        public Task<AST> Parse(IAdminDSLScanner scanner)
        {
            _scanner = scanner;
            _scanner.CurrentToken = _scanner.GetNextToken().GetAwaiter().GetResult();

            var root = ProgramRoot();

            if (_scanner.CurrentToken?.TokenType != TokenTypeEnum.EOF)
            {
                throw Error.ErrorMessages.InvalidSyntaxErr();
            }

            return TaskCache.ObjectValue(root)!;
        }        
    }
}
