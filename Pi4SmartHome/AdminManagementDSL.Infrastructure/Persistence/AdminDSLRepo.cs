using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Core.Configurations;
using AdminManagementDSL.Core.Entities;
using AdminManagementDSL.Core.Enums;
using Microsoft.Extensions.Options;

namespace AdminManagementDSL.Infrastructure.Persistence
{
    public class AdminDSLRepo : IAdminDSLRepo
    {
        private IDisposable? _sqlConnHandle;
        private SqlConnectionOptions SqlConnOptions;

        public AdminDSLRepo(IOptionsMonitor<SqlConnectionOptions> sqlOptions)
        {
            SqlConnOptions = sqlOptions.CurrentValue;
            _sqlConnHandle = sqlOptions.OnChange(OnSqlConnChange);
        }

        private void OnSqlConnChange(SqlConnectionOptions sqlConnOptions)
        {
            SqlConnOptions = sqlConnOptions;
        }

        public Task<IEnumerable<AdminDSL>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AdminDSL?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(AdminDSL item)
        {
            await using SqlConnection conn = new(SqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.AdminDSL_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DSLCode", item.DSLCode);
            cmd.Parameters.AddWithValue("@DSLStatus", item.DSLStatus);
            conn.Open();

            var result = await cmd.ExecuteNonQueryAsync();

            return result;
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
            await using SqlConnection conn = new(SqlConnOptions.SqlConnection);
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
