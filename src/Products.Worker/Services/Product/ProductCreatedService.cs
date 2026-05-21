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
    internal class ProductCreatedService : ProductServiceBase
    {
        public ProductCreatedService(ProductApiContext dbContext) : base(dbContext)
        {
        }


        public override string ConsumerBind { get => RabbitMQConfigure.ProductEventsRoutingKey.Created; }


        public override async Task ExecuteAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            //processamento

            await SaveLogAsync(eventArgs);
        }
    }
}
