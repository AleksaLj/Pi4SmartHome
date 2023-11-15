
namespace DeviceManagement.Application.Interfaces
{
    public interface IInsertRepo<in T> where T : class
    {
        Task<int> InsertAsync(T item);
    }
}
