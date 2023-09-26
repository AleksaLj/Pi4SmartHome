using System.Reflection;
using AdminManagementDSL.AdminDSL.Common.Extensions;
using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Infrastructure.Common.Extensions;
using AdminManagementDSL.Test;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Extensions;

var app = App.BuildServices((services, config) => 
{
    //RabbitMQ Services
    services.AddRabbitMQConfiguration(config);
    services.AddMessageConsumer<AdminDSLMessage>(getExchangeName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLExchangeName").Value!,
                                                 getExchangeQueueRoutingKey: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueRoutingKey").Value!,
                                                 getQueueName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueName").Value!);

    //AdminDSL Services
    services.AddAdminDSLParser();
    services.AddNodeVisitor();
    services.AddAdminDSLInterpreter();
    services.AddAdminDslRepo();

    //MediatR
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateAdminDSLCommand>());
});

var mediator = app.Services.GetService<IMediator>();
var consumerTest = new RabbitMQConsumerTest(app.Services.GetMessageConsumer<AdminDSLMessage>()!, mediator);
await consumerTest.ReceiveMessage();

//var parser = app.Services.GetAdminDSLParser();
//var interpreter = app.Services.GetAdminDSLInterpreter();

//var adminDSLTest = new AdminDSLTest();
//await adminDSLTest.TestAdminDSLExample(parser, interpreter);
