using Microsoft.EntityFrameworkCore;
using Product.Models;

namespace ProductApi.Data
{
    public class ProductApiContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source= product.sqlite");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
