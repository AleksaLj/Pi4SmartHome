using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Core.Configurations;
using AdminManagementDSL.Core.Entities;
using Microsoft.Extensions.Options;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AdminManagementDSL.Infrastructure.Persistence
{
    public class EstatesRepo : IEstatesRepo
    {
        private IDisposable? _sqlConnHandle;
        private SqlConnectionOptions SqlConnOptions;

        public EstatesRepo(IOptionsMonitor<SqlConnectionOptions> sqlOptions)
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
                SqlCommand cmd = new SqlCommand("mgmtdsl.Estates_DeleteById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstateId", id);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }

        public async Task<IEnumerable<Estates>> GetAllAsync()
        {
            var items = new List<Estates>();

            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Estates_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    var item = new Estates
                    {
                        EstateId = Convert.ToInt32(rdr["EstateId"]),
                        Name = rdr["Name"].ToString()!,
                        Addr = rdr["Addr"].ToString(),
                        Description = rdr["Description"].ToString(),
                        EstateTypeId = Convert.ToByte(rdr["EstateTypeId"])
                    };
                    items.Add(item);
                }

                return items;
            }
        }

        public async Task<Estates?> GetByIdAsync(object id)
        {
            Estates? item = default(Estates);

            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Estates_GetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstateId", id);
                conn.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    item = new Estates
                    {
                        EstateId = Convert.ToInt32(rdr["EstateId"]),
                        Name = rdr["Name"].ToString()!,
                        Addr = rdr["Addr"].ToString(),
                        Description = rdr["Description"].ToString(),
                        EstateTypeId = Convert.ToByte(rdr["EstateTypeId"])
                    };
                }

                return item;
            }
        }

        public async Task<int> InsertAsync(Estates item)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Estates_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", item.Name);
                if (!string.IsNullOrEmpty(item.Addr))
                {
                    cmd.Parameters.AddWithValue("@Addr", item.Addr);
                }
                if (!string.IsNullOrEmpty(item.Description))
                {
                    cmd.Parameters.AddWithValue("@Description", item.Description);
                }
                cmd.Parameters.AddWithValue("@EstateTypeId", item.EstateTypeId);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }

        public async Task<int> UpdateAsync(Estates item)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Estates_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EstateId", item.EstateId);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                if (!string.IsNullOrEmpty(item.Addr))
                {
                    cmd.Parameters.AddWithValue("@Addr", item.Addr);
                }
                if (!string.IsNullOrEmpty(item.Description))
                {
                    cmd.Parameters.AddWithValue("@Description", item.Description);
                }
                cmd.Parameters.AddWithValue("@EstateTypeId", item.EstateTypeId);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }
    }
}
