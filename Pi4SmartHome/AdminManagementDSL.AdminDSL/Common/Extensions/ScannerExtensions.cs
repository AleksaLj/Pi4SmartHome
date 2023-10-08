
using AdminManagementDSL.AdminDSL.Common.Core;

namespace AdminManagementDSL.AdminDSL.Common.Extensions
{
    internal static class ScannerExtensions
    {        
        internal static bool IsIdentifierValueTableKeyword(this string identifierValue)
        {
            return identifierValue is ReservedKeywords.TableKeywords.Keyword_EstatePart or ReservedKeywords.TableKeywords.Keyword_Estates or ReservedKeywords.TableKeywords.Keyword_EstateDevices or ReservedKeywords.TableKeywords.Keyword_EstateUsers;
        }

        internal static bool IsIdentifierValuePropertyKeyword(this string identifierValue)
        {
            return identifierValue is ReservedKeywords.PropertyKeywords.Keyword_EstateType or ReservedKeywords.PropertyKeywords.Keyword_Name or ReservedKeywords.PropertyKeywords.Keyword_Addr or ReservedKeywords.PropertyKeywords.Keyword_Description or ReservedKeywords.PropertyKeywords.Keyword_Users or ReservedKeywords.PropertyKeywords.Keyword_EstateParts or ReservedKeywords.PropertyKeywords.Keyword_DeviceType or ReservedKeywords.PropertyKeywords.Keyword_IsActive or ReservedKeywords.PropertyKeywords.Keyword_EstatePart;
        }

        internal static bool IsIdentifierValueTypeKeyword(this string identifierValue)
        {
            return identifierValue is ReservedKeywords.TypeKeywords.Keyword_Table or ReservedKeywords.TypeKeywords.Keyword_Field or ReservedKeywords.TypeKeywords.Keyword_Aggr;
        }

        internal static bool IsIdentifierValueAdminDSLKeyword(this string identifierValue)
        {
            return identifierValue is ReservedKeywords.AdminDSLKeywords.Keyword_Define or ReservedKeywords.AdminDSLKeywords.Keyword_Begin or ReservedKeywords.AdminDSLKeywords.Keyword_End or ReservedKeywords.AdminDSLKeywords.Keyword_Provision;
        }

        internal static bool IsIdentifierValueLogicalOperatorKeyword(this string identifierValue)
        {
            return identifierValue == ReservedKeywords.LogicalOperatorKeywords.Keyword_And;
        }
    }
}
