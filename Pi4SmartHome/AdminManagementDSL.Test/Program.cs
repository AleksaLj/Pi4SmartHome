using AdminManagementDSL.AdminDSL.Common.Extensions;
using AdminManagementDSL.Application.AdminDSL.Commands;
using AdminManagementDSL.Infrastructure.Common.Extensions;
using AdminManagementDSL.Test;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Common.Extensions;

var app = App.BuildServices((services, config) => 
{
    services.AddSqlConnOptions(config);
    services.AddRabbitMQConfiguration(config);

    services.AddMessageConsumer<AdminDSLMessage>(getExchangeName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLExchangeName").Value!,
                                                 getExchangeQueueRoutingKey: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueRoutingKey").Value!,
                                                 getQueueName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueName").Value!);

    services.AddMessageProducer<AdminDSLInterpreterEndMessage>(getExchangeName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndExchangeName").Value!,
        getExchangeQueueRoutingKey: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueRoutingKey").Value!,
        getQueueName: () => config.GetSection("rabbitMQ:Configuration:AdminManagementDSLEndQueueName").Value!);

    services.AddAdminDSLParser();
    services.AddNodeVisitor();
    services.AddAdminDSLInterpreter();
    services.AddAdminDslRepo();

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateAdminDSLCommand>());
});

var parser = app.Services.GetAdminDSLParser();
var interpreter = app.Services.GetAdminDSLInterpreter();

var mediator = app.Services.GetService<IMediator>();
var consumerTest = new RabbitMQConsumerTest(app.Services.GetMessageConsumer<AdminDSLMessage>()!, 
                                            mediator!, 
                                            parser!, 
                                            interpreter!, 
                                            app.Services.GetService<ILogger<AdminManagementDSLMsgHandler>>()!,
                                            app.Services.GetMessageProducer<AdminDSLInterpreterEndMessage>()!);
await consumerTest.ReceiveMessage();


//var adminDSLTest = new AdminDSLTest();
//await adminDSLTest.TestAdminDSLExample(parser, interpreter);
