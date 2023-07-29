
using AdminManagementDSL.AdminDSL.Common.Core;

namespace AdminManagementDSL.AdminDSL.Common.Extensions
{
    internal static class ScannerExtensions
    {        
        internal static bool IsIdentifierValueTableKeyword(this string identifierValue)
        {
            if (identifierValue == ReservedKeywords.TableKeywords.Keyword_EstatePart ||
                identifierValue == ReservedKeywords.TableKeywords.Keyword_Estates ||
                identifierValue == ReservedKeywords.TableKeywords.Keyword_EstateDevices ||
                identifierValue == ReservedKeywords.TableKeywords.Keyword_EstateUsers)
            {
                return true;
            }

            return false;
        }

        internal static bool IsIdentifierValuePropertyKeyword(this string identifierValue)
        {
            if (identifierValue == ReservedKeywords.PropertyKeywords.Keyword_EstateType ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_Name ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_Addr ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_Description ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_Users ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_EstateParts ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_DeviceType ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_IsActive ||
                identifierValue == ReservedKeywords.PropertyKeywords.Keyword_EstatePart
                )
            {
                return true;
            }

            return false;
        }

        internal static bool IsIdentifierValueTypeKeyword(this string identifierValue)
        {
            if (identifierValue == ReservedKeywords.TypeKeywords.Keyword_Table ||
                identifierValue == ReservedKeywords.TypeKeywords.Keyword_Field ||
                identifierValue == ReservedKeywords.TypeKeywords.Keyword_Aggr)
            {
                return true;
            }

            return false;
        }

        internal static bool IsIdentifierValueAdminDSLKeyword(this string identifierValue)
        {
            if (identifierValue == ReservedKeywords.AdminDSLKeywords.Keyword_Define ||
                identifierValue == ReservedKeywords.AdminDSLKeywords.Keyword_Begin ||
                identifierValue == ReservedKeywords.AdminDSLKeywords.Keyword_End ||
                identifierValue == ReservedKeywords.AdminDSLKeywords.Keyword_Provision)
            {
                return true;
            }

            return false;
        }
    }
}
