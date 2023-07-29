using Microsoft.Extensions.Logging;

namespace Pi4SmartHome.Core.Implementations
{
    public class ServiceBase : DisposableObject
    {
        protected IServiceProvider ServiceProvider { get; }
        protected ILogger Log { get; }

        public ServiceBase(IServiceProvider serviceProvider, ILogger log)
        {
            ServiceProvider = serviceProvider;
            Log = log;
        }
    }
}
