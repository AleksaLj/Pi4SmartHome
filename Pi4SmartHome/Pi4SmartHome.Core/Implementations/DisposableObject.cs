using Pi4SmartHome.Core.Interfaces;

namespace Pi4SmartHome.Core.Implementations
{
    public class DisposableObject : IDisposableObject
    {
        bool isDisposed = false;
        bool IDisposableObject.IsDisposed => isDisposed;
        protected bool IsDisposed => isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                //free managed code..
            }

            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposableObject()
        {
            Dispose(false);
        }
    }
}
