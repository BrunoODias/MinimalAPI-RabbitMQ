using Product.Api;
using ProductApi.Data;
using RabbitMQ.Client.Events;

namespace Products.Worker.Services.Product
{
    internal class ProductEditedService : ProductServiceBase
    {
        public ProductEditedService(ProductApiContext dbContext) : base(dbContext)
        {
        }
        public override string ConsumerBind { get => RabbitMQConfigure.ProductEventsRoutingKey.Edited ; }


        public override async Task ExecuteAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            //processamento

            await SaveLogAsync(eventArgs);
        }
    }
}
