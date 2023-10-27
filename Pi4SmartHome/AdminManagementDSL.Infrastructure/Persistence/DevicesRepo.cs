using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Core.Configurations;
using AdminManagementDSL.Core.Entities;
using AdminManagementDSL.Infrastructure.Common.Helper;
using Microsoft.Extensions.Options;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AdminManagementDSL.Infrastructure.Persistence
{
    public class DevicesRepo : IDevicesRepo
    {
        private IDisposable? _sqlConnHandle;
        private SqlConnectionOptions SqlConnOptions;

        public DevicesRepo(IOptionsMonitor<SqlConnectionOptions> sqlOptions)
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
            await using var conn = new SqlConnection(SqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.Devices_DeleteById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DeviceId", id);
            conn.Open();

            var result = await cmd.ExecuteNonQueryAsync();

            return result;
        }

        public async Task<IEnumerable<Devices>> GetAllAsync()
        {
            var items = new List<Devices>();

            await using SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.Devices_GetAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();

            var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                var item = new Devices
                {
                    DeviceId = Convert.ToInt32(rdr["DeviceId"]),
                    IsActive = Convert.ToChar(rdr["IsActive"]),
                    TimeActivated = PersistenceHelper.GetDateTime(rdr, "TimeActivated"),
                    TimeDeactivated = PersistenceHelper.GetDateTime(rdr, "TimeDeactivated"),
                    EstateId = Convert.ToInt32(rdr["EstateId"]),
                    EstatePartId = PersistenceHelper.GetInt(rdr, "EstatePartId"),
                    DeviceTypeId = Convert.ToInt32(rdr["DeviceTypeId"])
                };
                items.Add(item);
            }

            return items;
        }

        public async Task<Devices?> GetByIdAsync(object id)
        {
            var item = default(Devices?);

            await using var conn = new SqlConnection(SqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.Devices_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DeviceId", id);
            conn.Open();

            var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                item = new Devices
                {
                    DeviceId = Convert.ToInt32(rdr["DeviceId"]),
                    IsActive = Convert.ToChar(rdr["IsActive"]),
                    TimeActivated = PersistenceHelper.GetDateTime(rdr, "TimeActivated"),
                    TimeDeactivated = PersistenceHelper.GetDateTime(rdr, "TimeDeactivated"),
                    EstateId = Convert.ToInt32(rdr["EstateId"]),
                    EstatePartId = PersistenceHelper.GetInt(rdr, "EstatePartId"),
                    DeviceTypeId = Convert.ToInt32(rdr["DeviceTypeId"])
                };
            }

            return item;
        }

        public async Task<int> InsertAsync(Devices item)
        {
            await using SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.Devices_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IsActive", item.IsActive);
            if (item.TimeActivated.HasValue)
            {
                cmd.Parameters.AddWithValue("@TimeActivated", item.TimeActivated);
            }
            if (item.TimeDeactivated.HasValue)
            {
                cmd.Parameters.AddWithValue("@TimeDeactivated", item.TimeDeactivated);
            }
            cmd.Parameters.AddWithValue("@EstateId", item.EstateId);
            if (item.EstatePartId.HasValue)
            {
                cmd.Parameters.AddWithValue("@EstatePartId", item.EstatePartId);
            }
            cmd.Parameters.AddWithValue("@DeviceTypeId", item.DeviceTypeId);
            conn.Open();

            var result = await cmd.ExecuteNonQueryAsync();

            return result;
        }

        public async Task<int> UpdateAsync(Devices item)
        {
            await using SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.Devices_Update", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DeviceId", item.DeviceId);
            cmd.Parameters.AddWithValue("@IsActive", item.IsActive);
            if (item.TimeActivated.HasValue)
            {
                cmd.Parameters.AddWithValue("@TimeActivated", item.TimeActivated);
            }
            if (item.TimeDeactivated.HasValue)
            {
                cmd.Parameters.AddWithValue("@TimeDeactivated", item.TimeDeactivated);
            }
            cmd.Parameters.AddWithValue("@EstateId", item.EstateId);
            if (item.EstatePartId.HasValue)
            {
                cmd.Parameters.AddWithValue("@EstatePartId", item.EstatePartId);
            }
            cmd.Parameters.AddWithValue("@DeviceTypeId", item.DeviceTypeId);
            conn.Open();

            var result = await cmd.ExecuteNonQueryAsync();

            return result;
        }

        ~DevicesRepo()
        {
            _sqlConnHandle?.Dispose();
        }
    }
}
