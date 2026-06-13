using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using Product.Models;
using System.Security.Principal;
using System.Linq;
using RabbitMQ.Client;
using System.Text.Json;
using Product.Api;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ProductApi.Routes
{
    public static class ProductRoute
    {
        public static void AddProductRoutes(this WebApplication app)
        {
            var productRoute = app.MapGroup("product");

            //RabbitMQ
            var rabbit = app.Services.GetRequiredService<RabbitMQConfigure>();

            //Permissão
            productRoute.MapGet("/products", async ([FromQuery] string? productName, [FromQuery] decimal? productPrice, [FromServices] ProductApiContext context) =>
            {
                var query = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(productName))
                    query = query.Where(x => x.Name == productName);

                if (productPrice.HasValue && productPrice != 0)
                    query = query.Where(x => x.Price == productPrice.Value);

                return Results.Ok(await query.ToListAsync());
            });

            //Permissão
            productRoute.MapPost("/product", async ([FromBody] ProductRecord productReq, HttpContext httpContext, [FromServices] ProductRabbitSender rabbitSender, [FromServices] ProductApiContext context) =>
            {
                //Validação/Injação
                ProductModel product = new ProductModel(productReq.name, productReq.price);

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();

                await rabbitSender.SendMessageAsnyc(httpContext, product, RabbitMQConfigure.ProductEventsRoutingKey.Created);

                return Results.Ok(product);
            });

            //Permissão
            productRoute.MapPut("/product{id:guid}", async (Guid Id, [FromBody] ProductRecord productReq, HttpContext httpContext, [FromServices] ProductRabbitSender rabbitSender, [FromServices] ProductApiContext context) =>
            {
                var productEntry = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);

                if (productEntry == null)
                    return Results.NotFound();

                var productBefore = JsonSerializer.Deserialize<ProductModel>(JsonSerializer.Serialize(productEntry));

                productEntry.Update(productReq);
                await context.SaveChangesAsync();

                await rabbitSender.SendMessageAsnyc(httpContext, productBefore, RabbitMQConfigure.ProductEventsRoutingKey.Edited, productEntry);

                return Results.Ok();
            });

            //Permissão
            productRoute.MapDelete("/product{id:guid}", async (Guid Id, HttpContext httpContext, [FromServices] ProductRabbitSender rabbitSender, [FromServices] ProductApiContext context) =>
            {
                var productEntry = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);

                if (productEntry == null)
                    return Results.NotFound();

                //Soft Delete
                productEntry.Delete();
                await context.SaveChangesAsync();

                await rabbitSender.SendMessageAsnyc(httpContext, productEntry, RabbitMQConfigure.ProductEventsRoutingKey.Deleted);

                return Results.Ok();
            });
        }
    }
}
