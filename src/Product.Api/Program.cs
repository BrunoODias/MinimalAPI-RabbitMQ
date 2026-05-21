using ProductApi.Data;
using ProductApi.Routes;
using RabbitMQ.AMQP.Client.Impl;
using RabbitMQ.AMQP.Client;
using Product.Models;
using RabbitMQ.Client;
using System.Text.Json;
using System.Threading.Channels;
using Product.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProductApiContext>();
builder.Services.AddSingleton<RabbitMQConfigure>();
builder.Services.AddScoped<ProductRabbitSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddProductRoutes();

app.Run();
