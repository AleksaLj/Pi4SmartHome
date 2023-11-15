
namespace DeviceManagement.Application.Interfaces
{
    public interface IUpdateRepo<in T> where T : class
    {
        Task<int> UpdateAsync(T item);
    }
}
