
namespace AdminManagementDSL.AdminDSL.Common.Core
{
    public static class ReservedKeywords
    {
        public static class TableKeywords
        {
            public const string Keyword_Estates = "ESTATE";
            public const string Keyword_EstateUsers = "ESTATE_USERS";
            public const string Keyword_EstatePart = "ESTATE_PARTS";
            public const string Keyword_EstateDevices = "ESTATE_DEVICES";
        }

        public static class PropertyKeywords
        {
            public const string Keyword_EstateType = "EstateType";
            public const string Keyword_Name = "Name";
            public const string Keyword_Addr = "Addr";
            public const string Keyword_Description = "Description";
            public const string Keyword_Users = "Users";
            public const string Keyword_EstateParts = "EstateParts";
            public const string Keyword_DeviceType = "DeviceType";
            public const string Keyword_IsActive = "IsActive";
            public const string Keyword_EstatePart = "EstatePart";
        }

        public static class TypeKeywords
        {
            public const string Keyword_Table = "TABLE";
            public const string Keyword_Field = "FIELD";
            public const string Keyword_Aggr = "AGGR";
        }

        public static class AdminDSLKeywords
        {
            public const string Keyword_Provision = "PI4SMARTHOMEADMIN.PROVISION";
            public const string Keyword_Begin = "BEGIN";
            public const string Keyword_End = "END";
            public const string Keyword_Define = "DEFINE";
        }

        public static class LogicalOperatorKeywords
        {
            public const string Keyword_And = "AND";
        }
    }
}
