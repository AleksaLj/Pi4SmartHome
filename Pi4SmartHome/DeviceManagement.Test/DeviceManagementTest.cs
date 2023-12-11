using DeviceManagement.Service.Implementations;
using DeviceManagement.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;

namespace DeviceManagement.Test
{
    public class DeviceManagementTest
    {
        public async Task TestDeviceManagement(IServiceProvider services)
        {
            var deviceIds = new List<string> { "test-device-1", "test-device-2" };

            var msgConsumer = services.GetService<IMessageConsumer<AdminDSLInterpreterEndMessage>>()!;
            var msgHandler = services.GetService<IAdminDSLInterpreterEndMsgHandler>()!;
            var logger = services.GetService<ILogger>()!;
            var deviceManagementService = new DeviceManagementService(services, logger, msgConsumer, msgHandler);

            var adminDslInterpreterEndMessage = new AdminDSLInterpreterEndMessage(Guid.Parse("1B1E257E-372A-4F1E-85B3-D45EEA45804E"), deviceIds);

            await msgHandler.OnMessage(adminDslInterpreterEndMessage, this);
        }
    }
}
