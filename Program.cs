using ProductApi.Data;
using ProductApi.Routes;
using RabbitMQ.AMQP.Client.Impl;
using RabbitMQ.AMQP.Client;
using Product.Models;
using RabbitMQ.Client;
using System.Text.Json;
using System.Threading.Channels;
using Product.RabbitMQConfigure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProductApiContext>();
builder.Services.AddSingleton<RabbitMQConfigure>();

var app = builder.Build();

//RabbitMQ
var rabbit = app.Services.GetRequiredService<RabbitMQConfigure>();

//Envio da mensagem de pedido
ProductModel product = new ProductModel("Feijao", 5);
byte[] bodyBytes = JsonSerializer.SerializeToUtf8Bytes(product);

await rabbit.channel.BasicPublishAsync(
    exchange: Product.RabbitMQConfigure.RabbitMQConfigure.ProductExangeName,
    routingKey: Product.RabbitMQConfigure.RabbitMQConfigure.ProductRoutingKey,
    mandatory: false,
    basicProperties: new BasicProperties()
    {
        Persistent = true,
        ContentType = "application/json",
        ContentEncoding = "utf-8"
    },
    body: bodyBytes
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddProductRoutes();

app.Run();
