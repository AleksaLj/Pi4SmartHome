using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pi4SmartHomeDSL.Test
{
    public class AppInstance
    {
        private readonly IHost _host;

        public AppInstance(IHost host)
        {
            _host = host;
        }

        public IServiceProvider Services => _host.Services;
        public IConfiguration? Configuration => _host.Services.GetService<IConfiguration>();
    }

    public static class App
    {
        public static AppInstance BuildInstance(Action<IServiceCollection, IConfiguration> configure)
        {
            var serviceHost = new HostBuilder().Build();

            return new AppInstance(serviceHost);
        }
    }
}
