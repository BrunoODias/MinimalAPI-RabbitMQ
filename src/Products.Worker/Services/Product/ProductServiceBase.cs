using Microsoft.EntityFrameworkCore;
using Product.Api;
using Product.Models;
using ProductApi.Data;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Products.Worker.Services.Product
{
    internal abstract class ProductServiceBase : IService
    {
        public ProductServiceBase(ProductApiContext dbContext)
        {
            DbContext = dbContext;
        }

        protected ProductApiContext DbContext { get; }
        public abstract string ConsumerBind { get; }

        internal async Task SaveLogAsync(BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            var productLog = JsonSerializer.Deserialize<ProductLog>(message);

            DbContext.ProductLogs.Add(productLog);
            await DbContext.SaveChangesAsync();
        }

        public abstract Task ExecuteAsync(object sender, BasicDeliverEventArgs eventArgs);
    }
}
