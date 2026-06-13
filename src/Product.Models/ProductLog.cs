using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Product.Models
{
    public class ProductLog
    {
        public ProductLog()
        {
            
        }
        public int Id { get; init; }
        public Guid ProductId { get; init; }
        public string IpAddress { get; init; }
        public DateTime DateTime { get; init; }
        public string Operation { get; init; }
        public string Productbefore { get; init; }
        public string ProductAfter { get; init; }
        public ProductLog(ProductModel productBefore, string ipAddress, string operation, DateTime operationTime, ProductModel productAfter)
        {
            ProductId = productBefore.Id;
            Productbefore = JsonSerializer.Serialize(productBefore);
            ProductAfter = JsonSerializer.Serialize(productAfter);
            IpAddress = ipAddress;
            Operation = operation;
            DateTime = operationTime;
        }
    }
}
