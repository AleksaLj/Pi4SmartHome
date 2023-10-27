using System.Data;
using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Dto;
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

        public Task<IEnumerable<Core.Entities.AdminDSL>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Core.Entities.AdminDSL?> GetByIdAsync(object id)
        {
            Core.Entities.AdminDSL? item = default;

            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.AdminDSL_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();

            var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                item = new Core.Entities.AdminDSL
                {
                    AdminDSLId = Convert.ToInt32(rdr["AdminDSLId"]),
                    DSLCode = rdr["DSLCode"].ToString()!,
                    DSLStatus = Convert.ToByte(rdr["DSLStatus"]),
                    AdminDSLGuid = Guid.Parse(rdr["AdminDSLGuid"].ToString()!)
                };
            }

            return item;
        }

        public async Task<int> InsertAsync(Core.Entities.AdminDSL item)
        {
            await using SqlConnection conn = new(_sqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.AdminDSL_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DSLCode", item.DSLCode);
            cmd.Parameters.AddWithValue("@DSLStatus", item.DSLStatus);
            cmd.Parameters.AddWithValue("@DSLGuid", item.AdminDSLGuid);
            var outputParam = new SqlParameter("@Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);
            conn.Open();

            await cmd.ExecuteNonQueryAsync();

            return (int)outputParam.Value;
        }

        public Task<int> UpdateAsync(Core.Entities.AdminDSL item)
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

        public async Task BulkInsertPi4SmartHomeInterpretedDataAsync(IEnumerable<SqlTableDto> sqlTables, int adminDslId)
        {
            foreach (var sqlTable in sqlTables)
            {
                switch (sqlTable.TableName)
                {
                    case ReservedKeywords.TableKeywords.Keyword_Estates:
                        await InsertEstatesInterpretedDataAsync(sqlTable, adminDslId);
                        break;
                    case ReservedKeywords.TableKeywords.Keyword_EstatePart:
                        await EstatePartsBulkInsertInterpretedDataAsync(sqlTable, adminDslId);
                        break;
                    case ReservedKeywords.TableKeywords.Keyword_EstateDevices:
                        await DevicesBulkInsertInterpretedDataAsync(sqlTable, adminDslId);
                        break;
                    case ReservedKeywords.TableKeywords.Keyword_EstateUsers:
                        await EstateUsersBulkInsertInterpretedDataAsync(sqlTable, adminDslId);
                        break;
                    default: return;
                }
            }
        }

        private async Task<int> InsertEstatesInterpretedDataAsync(SqlTableDto estate, int adminDslId)
        {
            if(!ValidateInputParams(estate)) return default;

            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.Estates_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            var estateNameCol = estate.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_Name);
            var estateAddrCol = estate.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_Addr);
            var estateDescriptionCol = estate.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_Description);
            var estatePartCol = estate.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_EstateType);

            cmd.Parameters.AddWithValue("@Name", estateNameCol?.Value);
            cmd.Parameters.AddWithValue("@Addr", estateAddrCol?.Value);
            cmd.Parameters.AddWithValue("@Description", estateDescriptionCol?.Value);
            cmd.Parameters.AddWithValue("@EstateTypeName", estatePartCol?.Value);
            cmd.Parameters.AddWithValue("@AdminDslId", adminDslId);

            conn.Open();

            return await cmd.ExecuteNonQueryAsync();
        }

        private async Task<int> EstatePartsBulkInsertInterpretedDataAsync(SqlTableDto estatePart, int adminDslId)
        {
            if (!ValidateInputParams(estatePart)) return default;

            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.EstatePart_BulkInsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            var estatePartsCol = estatePart.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_EstateParts);

            cmd.Parameters.AddWithValue("@EstateParts", estatePartsCol?.Value);
            cmd.Parameters.AddWithValue("@AdminDslId", adminDslId);

            conn.Open();

            return await cmd.ExecuteNonQueryAsync();
        }

        private async Task<int> DevicesBulkInsertInterpretedDataAsync(SqlTableDto device, int adminDslId)
        {
            if (!ValidateInputParams(device)) return default;

            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.Devices_BulkInsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            var deviceTypeCol = device.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_DeviceType);
            var isActiveCol = device.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_IsActive);
            var estatePartCol = device.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_EstatePart);

            cmd.Parameters.AddWithValue("@IsActive", (string?)isActiveCol?.Value == "true" ? "T" : "F");
            cmd.Parameters.AddWithValue("@TimeActivated", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@EstatePartName", estatePartCol?.Value);
            cmd.Parameters.AddWithValue("@DeviceTypeName", deviceTypeCol?.Value);
            cmd.Parameters.AddWithValue("@AdminDslId", adminDslId);

            conn.Open();

            return await cmd.ExecuteNonQueryAsync();
        }

        private async Task<int> EstateUsersBulkInsertInterpretedDataAsync(SqlTableDto device, int adminDslId)
        {
            if (!ValidateInputParams(device)) return default;

            await using var conn = new SqlConnection(_sqlConnOptions.SqlConnection);
            SqlCommand cmd = new("mgmtdsl.EstateUsers_BulkInsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            var usersCol = device.Rows.First().Columns
                .FirstOrDefault(q => q.Name == ReservedKeywords.PropertyKeywords.Keyword_Users);

            cmd.Parameters.AddWithValue("@EstateUsers", usersCol?.Value);
            cmd.Parameters.AddWithValue("@AdminDslId", adminDslId);

            conn.Open();

            return await cmd.ExecuteNonQueryAsync();
        }

        private static bool ValidateInputParams(SqlTableDto item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));
            return item.Rows.Count != 0;
        }

        ~AdminDSLRepo()
        {
            _sqlConnHandle?.Dispose();
        }
    }
}
