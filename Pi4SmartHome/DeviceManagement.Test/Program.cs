using DeviceManagement.Application.Interfaces;
using DeviceManagement.Application.Models;
using DeviceManagement.Infrastructure.Common.Extensions;
using DeviceManagement.Service.Extensions;
using DeviceManagement.Service.Implementations;
using DeviceManagement.Service.Interfaces;
using DeviceManagement.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Extensions;
using Pi4SmartHome.Core.RabbitMQ.Interfaces;
using Pi4SmartHomeDSL.DSL.Common.Extensions;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;

var app = App.BuildServices((services, config) =>
{
    services.AddSqlConnOptions(config);
    services.AddIoTHubConnection(config);

    services.AddMessageConsumer<AdminDSLInterpreterEndMessage>(getExchangeName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndExchangeName").Value!,
        getExchangeQueueRoutingKey: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueRoutingKey").Value!,
        getQueueName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueName").Value!);

    services.AddIoTDeviceRepo();

    services.AddDeviceManagementService();
    services.AddAdminDslInterpreterEndMsgHandler();
    services.AddDeviceProvisioningService();
    services.AddDeviceMessagingService();

    services.AddPi4SmartHomeDslScanner();
    services.AddPi4SmartHomeDslParser();
    services.AddPi4SmartHomeDslInterpreter();
    services.AddNodeVisitor();
});


var parser = app.Services.GetService<IPi4SmartHomeDslParser>();
var interpreter = app.Services.GetService<IPi4SmartHomeDslInterpreter>();
await Pi4SmartHomeDslTest.Pi4SmartHomeDslExampleTest(parser, interpreter);

var deviceManagementTest = new DeviceManagementTest();

//await deviceManagementTest.TestDeviceManagement(app.Services);

var messagingService = app.Services.GetService<IIoTHubCloudToDeviceMessagingService>();

await messagingService!.SendMessageToDeviceAsync(new IoTDeviceMessage
    ("test-device-1", "test", new Dictionary<string, string>()));

