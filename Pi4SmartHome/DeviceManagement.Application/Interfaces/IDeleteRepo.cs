
namespace DeviceManagement.Application.Interfaces
{
    public interface IDeleteRepo
    {
        Task<int> DeleteAsync(object id);
    }
}
