using System.Threading.Channels;
using CloudToDevice.Service.Extensions;
using CloudToDevice.Service.Implementations;
using CloudToDevice.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.RabbitMQ.Common.Extensions;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHomeDSL.DSL.Common.Extensions;
using Pi4SmartHomeDSL.Infrastructure.Common.Extensions;


var app = App.BuildServices((services, config) =>
{
    services.AddIoTHubConnection(config);
    services.AddRabbitMQConfiguration(config);

    services.AddPi4SmartHomeDslScanner();
    services.AddPi4SmartHomeDslParser();
    services.AddPi4SmartHomeDslInterpreter();
    services.AddNodeVisitor();

    services.AddMessageConsumer<CloudToDeviceMessage>(
        getExchangeName: () => config.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslExchangeName").Value!,
        getExchangeQueueRoutingKey: () =>
            config.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslQueueRoutingKey").Value!,
        getQueueName: () => config.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslQueueName").Value!);

    services.AddCloudToDeviceMessageHandler();
    services.AddCloudToDeviceService();
    services.AddDeviceMessagingService();
});


var test = new List<string>();
var empStr = string.Empty;

if (!string.IsNullOrWhiteSpace(empStr))
{
    test.Add(empStr);
}

Console.WriteLine(test.Count);



var parser = app.Services.GetPi4SmartHomeDslParser();
var interpreter = app.Services.GetPi4SmartHomeDslInterpreter();
var iotCloudToDeviceService = app.Services.GetDeviceMessagingService();

var consumerTest = new RabbitMqConsumerTest(app.Services.GetMessageConsumer<CloudToDeviceMessage>()!,
                                            iotCloudToDeviceService, 
                                            interpreter, 
                                            parser,
                                      app.Services.GetService<ILogger<CloudToDeviceMessageHandler>>()!);

await consumerTest.ReceiveMessage();
