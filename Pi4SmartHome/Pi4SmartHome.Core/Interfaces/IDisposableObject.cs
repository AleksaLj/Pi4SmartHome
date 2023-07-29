
namespace Pi4SmartHome.Core.Interfaces
{
    public interface IDisposableObject : IDisposable
    {
        bool IsDisposed { get; }
    }
}
