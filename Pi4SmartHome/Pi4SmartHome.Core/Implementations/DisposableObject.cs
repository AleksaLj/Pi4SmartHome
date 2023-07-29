using Pi4SmartHome.Core.Interfaces;

namespace Pi4SmartHome.Core.Implementations
{
    public class DisposableObject : IDisposableObject
    {
        bool isDisposed = false;
        bool IDisposableObject.IsDisposed => IsDisposed;
        protected bool IsDisposed => isDisposed;

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                Dispose(true);
                isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~DisposableObject()
        {
            Dispose(false);
        }
    }
}
