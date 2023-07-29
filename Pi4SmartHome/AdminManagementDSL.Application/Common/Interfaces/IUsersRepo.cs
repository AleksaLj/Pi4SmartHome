using AdminManagementDSL.Core.Entities;

namespace AdminManagementDSL.Application.Common.Interfaces
{
    public interface IUsersRepo : IRepository<Users>
    {
        Task<Users?> SelectUserByEmailAsync(string email);

        Task<bool> CheckIfEmailIsActivatedAsync(string email);

        Task<bool> CheckSignInKeyForEmailAsync(string email, string signInKey);

        Task<int> VerifyEmailAsync(string email);

        Task<int> UpdateUserSignInKeyAsync(string email, string signInKey);
    }
}
