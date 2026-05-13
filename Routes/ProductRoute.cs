using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using System.Security.Principal;

namespace ProductApi.Routes
{
    public static class ProductRoute
    {
        public static void AddProductRoutes(this WebApplication app)
        {
            var productRoute = app.MapGroup("product");

            //Permissão
            productRoute.MapGet("s", async (ProductApiContext context) =>{
                return Results.Ok(await context.Products.ToListAsync());
            });

            //Permissão
            productRoute.MapPost("", async (ProductRecord productReq, ProductApiContext context) => {
                //Validação/Injação
                ProductModel product = new ProductModel(productReq.name, productReq.price);

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();

                return Results.Ok(product);
            });

            //Permissão
            productRoute.MapPut("{id:guid}", async (Guid Id, ProductRecord productReq, ProductApiContext context) => {
                var productEntry = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);

                if (productEntry == null)
                    return Results.NotFound();

                productEntry.Update(productReq);
                await context.SaveChangesAsync();

                return Results.Ok();
            });

            //Permissão
            productRoute.MapDelete("{id:guid}", async (Guid Id, ProductApiContext context) => {
                var productEntry = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);

                if (productEntry == null)
                    return Results.NotFound();

                //Soft Delete
                productEntry.Delete();
                await context.SaveChangesAsync();

                return Results.Ok();
            });
        }
    }
}
