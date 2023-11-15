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
});


var deviceManagementTest = new DeviceManagementTest();

deviceManagementTest.TestDeviceManagement(app.Services);

