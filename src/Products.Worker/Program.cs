using Products.Worker;
using Product.Api;
using Products.Worker.Services;
using Products.Worker.Services.Product;
using ProductApi.Data;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // RabbitMQ

        services.AddDbContext<ProductApiContext>();
        services.AddSingleton<RabbitMQConfigure>();

        services.AddScoped<IService, ProductCreatedService>();
        services.AddScoped<IService, ProductEditedService>();
        services.AddScoped<IService, ProductDeletedService>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
