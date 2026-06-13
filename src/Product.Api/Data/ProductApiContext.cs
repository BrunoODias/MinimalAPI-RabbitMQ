using Microsoft.EntityFrameworkCore;
using Product.Models;

namespace ProductApi.Data
{
    public class ProductApiContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductLog> ProductLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source= C:\\Users\\Bruno\\source\\repos\\MinimalAPI-C-\\product.sqlite");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
