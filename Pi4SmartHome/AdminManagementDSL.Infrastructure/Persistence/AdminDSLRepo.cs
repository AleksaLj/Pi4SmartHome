using System.Data;
using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Core.Configurations;
using AdminManagementDSL.Core.Entities;
using AdminManagementDSL.Core.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace AdminManagementDSL.Infrastructure.Persistence
{
    public class AdminDSLRepo : IAdminDSLRepo
    {
        private IDisposable? _sqlConnHandle;
        private SqlConnectionOptions _sqlConnOptions;

        public AdminDSLRepo(IOptionsMonitor<SqlConnectionOptions> sqlOptions)
        {
            _sqlConnOptions = sqlOptions.CurrentValue;
            _sqlConnHandle = sqlOptions.OnChange(OnSqlConnChange);
        }

        private void OnSqlConnChange(SqlConnectionOptions sqlConnOptions)
        {
            _sqlConnOptions = sqlConnOptions;
        }

        public Task<IEnumerable<AdminDSL>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AdminDSL?> GetByIdAsync(object id)
        {
            AdminDSL? item = default;

            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.AdminDSL_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();

            var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                item = new AdminDSL
                {
                    DSLCode = rdr["DSLCode"].ToString()!,
                    DSLStatus = Convert.ToByte(rdr["DSLStatus"])
                };
            }

            return item;
        }

        public async Task<int> InsertAsync(AdminDSL item)
        {
            await using SqlConnection conn = new(_sqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.AdminDSL_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DSLCode", item.DSLCode);
            cmd.Parameters.AddWithValue("@DSLStatus", item.DSLStatus);
            var outputParam = new SqlParameter("@Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            conn.Open();

            await cmd.ExecuteNonQueryAsync();

            return (int)outputParam.Value;
        }

        public Task<int> UpdateAsync(AdminDSL item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAdminDSLStatusAsync(int adminDslId, DSLStatus status)
        {
            await using SqlConnection conn = new(_sqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.AdminDSL_UpdateStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AdminDSLId", adminDslId);
            cmd.Parameters.AddWithValue("@DSLStatus", (byte)status);
            conn.Open();

            var result = await cmd.ExecuteNonQueryAsync();

            return result;
        }
    }
}
