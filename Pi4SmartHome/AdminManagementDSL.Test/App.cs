using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AdminManagementDSL.Test
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

    internal static class App
    {
        public static AppInstance BuildServices(Action<IServiceCollection, IConfiguration> configure)
        {
            var serviceHost = new HostBuilder()
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConsole();
                })
                .ConfigureServices((host, services) => 
                {
                    configure?.Invoke(services, host.Configuration);
                })
                .Build();

            return new AppInstance(serviceHost);
        }
    }
}
