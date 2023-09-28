using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Core.Configurations;
using AdminManagementDSL.Core.Entities;
using Microsoft.Extensions.Options;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AdminManagementDSL.Infrastructure.Persistence
{
    public class EstatePartRepo : IEstatePartRepo
    {
        private IDisposable? _sqlConnHandle;
        private SqlConnectionOptions SqlConnOptions;

        public EstatePartRepo(IOptionsMonitor<SqlConnectionOptions> sqlOptions)
        {
            SqlConnOptions = sqlOptions.CurrentValue;
            _sqlConnHandle = sqlOptions.OnChange(OnSqlConnChange);
        }

        private void OnSqlConnChange(SqlConnectionOptions sqlConnOptions)
        {
            SqlConnOptions = sqlConnOptions;
        }

        public async Task<int> DeleteAsync(object id)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.EstatePart_DeleteById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstatePartId", id);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }

        public async Task<IEnumerable<EstatePart>> GetAllAsync()
        {
            var items = new List<EstatePart>();

            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.EstatePart_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    var item = new EstatePart
                    {
                        EstatePartId = Convert.ToInt32(rdr["EstatePartId"]),
                        EstatePartName = rdr["EstatePartName"].ToString(),
                        EstateId = Convert.ToInt32(rdr["EstateId"])
                    };
                    items.Add(item);
                }

                return items;
            }
        }

        public async Task<EstatePart?> GetByIdAsync(object id)
        {
            EstatePart? item = default(EstatePart);

            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.EstatePart_GetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstatePartId", id);
                conn.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    item = new EstatePart
                    {
                        EstatePartId = Convert.ToInt32(rdr["EstatePartId"]),
                        EstatePartName = rdr["EstatePartName"].ToString(),
                        EstateId = Convert.ToInt32(rdr["EstateId"])
                    };
                }

                return item;
            }
        }

        public async Task<int> InsertAsync(EstatePart item)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.EstatePart_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(item.EstatePartName))
                {
                    cmd.Parameters.AddWithValue("@EstatePartName", item.EstatePartName);
                }
                cmd.Parameters.AddWithValue("@EstateId", item.EstateId);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }

        public async Task<int> UpdateAsync(EstatePart item)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.EstatePart_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstatePartId", item.EstatePartId);
                if (!string.IsNullOrEmpty(item.EstatePartName))
                {
                    cmd.Parameters.AddWithValue("@EstatePartName", item.EstatePartName);
                }
                cmd.Parameters.AddWithValue("@EstateId", item.EstateId);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }
    }
}
