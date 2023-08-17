using AdminManagementDSL.AdminDSL.Common.Extensions;
using AdminManagementDSL.Test;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Extensions;

var app = App.BuildServices((services, config) => 
{
    //RabbitMQ Services
    services.AddRabbitMQConfiguration(config);
    services.AddMessageConsumer<AdminDSLMessage>(getQueueName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueName").Value!);

    //AdminDSL Services
    services.AddAdminDSLParser();
    services.AddNodeVisitor();
    services.AddAdminDSLInterpreter();
});



var consumerTest = new RabbitMQConsumerTest(app.Services.GetMessageConsumer<AdminDSLMessage>()!);
consumerTest.ReceiveMessage();

var parser = app.Services.GetAdminDSLParser();
var interpreter = app.Services.GetAdminDSLInterpreter();

var adminDSLTest = new AdminDSLTest();
await adminDSLTest.TestAdminDSLExample(parser, interpreter);
