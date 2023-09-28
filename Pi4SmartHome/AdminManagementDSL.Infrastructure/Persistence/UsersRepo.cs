using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Core.Configurations;
using AdminManagementDSL.Core.Entities;
using AdminManagementDSL.Infrastructure.Common.Helper;
using Microsoft.Extensions.Options;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AdminManagementDSL.Infrastructure.Persistence
{
    public class UsersRepo : IUsersRepo
    {
        private IDisposable? _sqlConnHandle;
        private SqlConnectionOptions SqlConnOptions;

        public UsersRepo(IOptionsMonitor<SqlConnectionOptions> sqlOptions)
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
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_DeleteById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", id);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }

        public async Task<Users?> SelectUserByEmailAsync(string email)
        {
            Users? item = default(Users);

            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_SelectByEmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    item = new Users
                    {
                        UserId = Convert.ToInt32(rdr["UserId"]),
                        FirstName = rdr["FirstName"].ToString(),
                        LastName = rdr["LastName"].ToString(),
                        Addr = rdr["Addr"].ToString(),
                        City = rdr["City"].ToString(),
                        Country = rdr["Country"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        Email = rdr["Email"].ToString()!,
                        BirthDate = PersistenceHelper.GetDateTime(rdr, "BirthDate")
                    };
                }

                return item;
            }
        }

        public async Task<bool> CheckIfEmailIsActivatedAsync(string email)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_CheckIfEmailIsActivated", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);
                conn.Open();

                await cmd.ExecuteNonQueryAsync();
                if ((int)outParam.Value == 0)
                    return false;

                return true;
            }
        }

        public async Task<bool> CheckSignInKeyForEmailAsync(string email, string signInKey)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_CheckSignInKeyForEmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@SignInKey", signInKey);
                SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);
                conn.Open();

                await cmd.ExecuteNonQueryAsync();

                return (int)outParam.Value != 0;
            }
        }

        public async Task<int> VerifyEmailAsync(string email)
        {
            await using var conn = new SqlConnection(SqlConnOptions.SqlConnection);
            var cmd = new SqlCommand("mgmtdsl.Users_VerifyEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);
            conn.Open();

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> UpdateUserSignInKeyAsync(string email, string signInKey)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_UpdateSignInKey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@SignInKey", signInKey);
                conn.Open();

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            var items = new List<Users>();

            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_GetAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    var item = new Users
                    {
                        UserId = Convert.ToInt32(rdr["UserId"]),
                        FirstName = rdr["FirstName"].ToString(),
                        LastName = rdr["LastName"].ToString(),
                        BirthDate = PersistenceHelper.GetDateTime(rdr, "BirthDate"),
                        Addr = rdr["Addr"].ToString(),
                        City = rdr["City"].ToString(),
                        Country = rdr["Country"].ToString(),
                        Email = rdr["Email"].ToString()!,
                        Phone = rdr["Phone"].ToString(),
                        GDPRFlag = Convert.ToChar(rdr["GDPRFlag"]),
                        EmailVerify = Convert.ToChar(rdr["EmailVerify"])
                    };
                    items.Add(item);
                }

                return items;
            }
        }

        public async Task<Users?> GetByIdAsync(object id)
        {
            Users? item = default(Users);

            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_GetById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", id);
                conn.Open();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    item = new Users
                    {
                        UserId = Convert.ToInt32(rdr["UserId"]),
                        FirstName = rdr["FirstName"].ToString(),
                        LastName = rdr["LastName"].ToString(),
                        BirthDate = PersistenceHelper.GetDateTime(rdr, "BirthDate"),
                        Addr = rdr["Addr"].ToString(),
                        City = rdr["City"].ToString(),
                        Country = rdr["Country"].ToString(),
                        Email = rdr["Email"].ToString()!,
                        Phone = rdr["Phone"].ToString(),
                        GDPRFlag = Convert.ToChar(rdr["GDPRFlag"]),
                        EmailVerify = Convert.ToChar(rdr["EmailVerify"])
                    };
                }

                return item;
            }
        }

        public async Task<int> InsertAsync(Users item)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(item.FirstName))
                {
                    cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                }
                if (!string.IsNullOrEmpty(item.LastName))
                {
                    cmd.Parameters.AddWithValue("@LastName", item.LastName);
                }
                if (item.BirthDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@BirthDate", item.BirthDate);
                }
                if (!string.IsNullOrEmpty(item.Addr))
                {
                    cmd.Parameters.AddWithValue("@Addr", item.Addr);
                }
                if (!string.IsNullOrEmpty(item.City))
                {
                    cmd.Parameters.AddWithValue("@City", item.City);
                }
                if (!string.IsNullOrEmpty(item.Country))
                {
                    cmd.Parameters.AddWithValue("@Country", item.Country);
                }
                cmd.Parameters.AddWithValue("@Email", item.Email);
                if (!string.IsNullOrEmpty(item.Phone))
                {
                    cmd.Parameters.AddWithValue("@Phone", item.Phone);
                }
                cmd.Parameters.AddWithValue("@Pswrd", item.Pswrd);
                cmd.Parameters.AddWithValue("@GDPRFlag", item.GDPRFlag);
                cmd.Parameters.AddWithValue("@SignInKey", item.SignInKey);
                cmd.Parameters.AddWithValue("@EmailVerify", item.EmailVerify);
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }

        public async Task<int> UpdateAsync(Users item)
        {
            using (SqlConnection conn = new SqlConnection(SqlConnOptions.SqlConnection))
            {
                SqlCommand cmd = new SqlCommand("mgmtdsl.Users_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", item.UserId);
                if (!string.IsNullOrEmpty(item.FirstName))
                {
                    cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                }
                if (!string.IsNullOrEmpty(item.LastName))
                {
                    cmd.Parameters.AddWithValue("@LastName", item.LastName);
                }
                if (item.BirthDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@BirthDate", item.BirthDate);
                }
                if (!string.IsNullOrEmpty(item.Addr))
                {
                    cmd.Parameters.AddWithValue("@Addr", item.Addr);
                }
                if (!string.IsNullOrEmpty(item.City))
                {
                    cmd.Parameters.AddWithValue("@City", item.City);
                }
                if (!string.IsNullOrEmpty(item.Country))
                {
                    cmd.Parameters.AddWithValue("@Country", item.Country);
                }
                if (!string.IsNullOrEmpty(item.Phone))
                {
                    cmd.Parameters.AddWithValue("@Phone", item.Phone);
                }
                conn.Open();

                var result = await cmd.ExecuteNonQueryAsync();

                return result;
            }
        }
    }
}
