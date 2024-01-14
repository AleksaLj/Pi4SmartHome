using Pi4SmartHome.Core.RabbitMQ.Common.Extensions;
using Pi4SmartHome.Core.RabbitMQ.Common.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMQConfiguration(builder.Configuration);

builder.Services.AddMessageProducer<CloudToDeviceMessage>(
    getExchangeName: () =>
        builder.Configuration.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslExchangeName").Value!,
    getExchangeQueueRoutingKey: () =>
        builder.Configuration.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslQueueRoutingKey").Value!,
    getQueueName: () => builder.Configuration.GetSection("rabbitMQ:Configuration:Pi4SmartHomeDslQueueName").Value!);

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
