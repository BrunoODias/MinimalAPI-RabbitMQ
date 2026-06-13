using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Product.Models
{
    public record ProductRecord(string name, decimal price);
    public class ProductModel
    {
        public ProductModel(string name, decimal price)
        {
            Name = name;
            Price = price;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public decimal Price { get; set; }

        public void Delete()
        {
            Name = $"Desativado - {Name}";
        }

        public void Update(ProductRecord product)
        {
            //Validations
            Name = product.name;
            Price = product.price;
        }
    }
}
