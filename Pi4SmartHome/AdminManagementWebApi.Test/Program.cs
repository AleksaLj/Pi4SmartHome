using Pi4SmartHome.Core.RabbitMQ.Common.Messages;
using Pi4SmartHome.Core.RabbitMQ.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMQConfiguration(builder.Configuration);

builder.Services.AddMessageProducer<AdminDSLMessage>(getExchangeName: () => builder.Configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLExchangeName").Value!,
                                                     getExchangeQueueRoutingKey: () => builder.Configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueRoutingKey").Value!,
                                                     getQueueName: () => builder.Configuration.GetSection("rabbitMQ:Configuration:AdminManagementDSLQueueName").Value!);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
