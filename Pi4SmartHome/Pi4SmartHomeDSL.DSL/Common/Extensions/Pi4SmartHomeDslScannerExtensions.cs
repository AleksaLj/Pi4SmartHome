using Pi4SmartHomeDSL.DSL.Common.Core;

namespace Pi4SmartHomeDSL.DSL.Common.Extensions
{
    internal static class Pi4SmartHomeDslScannerExtensions
    {
        internal static bool IsIdentifierValuePropertyKeyword(this string identifierValue)
        {
            return identifierValue is ReservedKeywords.Pi4SmartHomeDslPropertyKeywords.Keyword_MessageBody or
                                      ReservedKeywords.Pi4SmartHomeDslPropertyKeywords.Keyword_MessageProperties or
                                      ReservedKeywords.Pi4SmartHomeDslPropertyKeywords.Keyword_To;
        }

        internal static bool IsIdentifierValuePi4SmartHomeKeyword(this string identifierValue)
        {
            return identifierValue is ReservedKeywords.Pi4SmartHomeDslKeywords.Keyword_Begin or
                                      ReservedKeywords.Pi4SmartHomeDslKeywords.Keyword_End or
                                      ReservedKeywords.Pi4SmartHomeDslKeywords.Keyword_SendDeviceMsg;
        }
    }
}
