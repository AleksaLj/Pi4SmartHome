using System.Data;
using DeviceManagement.Application.Interfaces;
using DeviceManagement.Core.Configurations;
using DeviceManagement.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace DeviceManagement.Infrastructure.Persistence
{
    public class IoTDeviceRepo : IIoTDeviceRepo
    {
        private IDisposable? _sqlConnHandle;
        private SqlConnectionOptions _sqlConnOptions;

        public IoTDeviceRepo(IOptionsMonitor<SqlConnectionOptions> sqlOptions)
        {
            _sqlConnOptions = sqlOptions.CurrentValue;
            _sqlConnHandle = sqlOptions.OnChange(OnSqlConnChange);
        }

        private void OnSqlConnChange(SqlConnectionOptions sqlConnOptions)
        {
            _sqlConnOptions = sqlConnOptions;
        }

        public async Task<int> InsertAsync(IoTDevice item)
        {
            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("[devmgmt].[IoTDevice_Insert]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DeviceStatus", item.DeviceStatus);
            cmd.Parameters.AddWithValue("@ConnectionState", item.ConnectionState);
            cmd.Parameters.AddWithValue("@IoTHubDeviceName", item.IoTHubDeviceName);
            if (item.ActivationDate.HasValue)
            {
                cmd.Parameters.AddWithValue("@ActivationDate", item.ActivationDate);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ActivationDate", DBNull.Value);
            }

            if (item.DeactivationDate.HasValue)
            {
                cmd.Parameters.AddWithValue("@DeactivationDate", item.DeactivationDate);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DeactivationDate", DBNull.Value);
            }

            await conn.OpenAsync();

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> BulkInsertIoTDevicesAsync(IEnumerable<IoTDevice> items)
        {
            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("[devmgmt].[IoTDevice_BulkInsert]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            var dt = GetIoTDevicesDataTable(items);
            cmd.Parameters.AddWithValue("@devices", dt);

            conn.Open();

            return await cmd.ExecuteNonQueryAsync();
        }

        private static DataTable GetIoTDevicesDataTable(IEnumerable<IoTDevice> items)
        {
            var dt = new DataTable();

            dt.Columns.Add("DeviceStatus", typeof(byte));
            dt.Columns.Add("ConnectionState", typeof(byte));
            dt.Columns.Add("ActivationDate", typeof(DateTime?));
            dt.Columns.Add("DeactivationDate", typeof(DateTime?));
            dt.Columns.Add("IoTHubDeviceName", typeof(string));

            foreach (var item in items)
            {
                dt.Rows.Add(item.DeviceStatus, item.ConnectionState, item.ActivationDate, item.DeactivationDate, item.IoTHubDeviceName);
            }

            return dt;
        }

        ~IoTDeviceRepo()
        {
            _sqlConnHandle?.Dispose();
        }
    }
}
