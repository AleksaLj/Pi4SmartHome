
namespace DeviceManagement.Application.Interfaces
{
    public interface IReadRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
    }
}
