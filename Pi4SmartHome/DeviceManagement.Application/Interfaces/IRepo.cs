
namespace DeviceManagement.Application.Interfaces
{
    public interface IRepo<T> : IReadRepo<T>, IInsertRepo<T>, IUpdateRepo<T>, IDeleteRepo where T : class
    {
        
    }
}
