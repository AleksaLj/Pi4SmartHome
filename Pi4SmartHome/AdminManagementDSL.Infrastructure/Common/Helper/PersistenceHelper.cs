using Microsoft.Data.SqlClient;

namespace AdminManagementDSL.Infrastructure.Common.Helper
{
    public static class PersistenceHelper
    {
        public static DateTime? GetDateTime(SqlDataReader rdr, string column)
        {
            if (rdr[column] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(rdr[column]);
        }

        public static int? GetInt(SqlDataReader rdr, string column)
        {
            if (rdr[column] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(rdr[column]);
        }
    }
}
