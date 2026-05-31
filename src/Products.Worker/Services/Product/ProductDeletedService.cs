using Product.Api;
using ProductApi.Data;
using RabbitMQ.Client.Events;

namespace Products.Worker.Services.Product
{
    class ProductDeletedService : ProductServiceBase
    {
        public ProductDeletedService(ProductApiContext dbContext) : base(dbContext)
        {
        }
        public override string ConsumerBind { get => RabbitMQConfigure.ProductEventsRoutingKey.Deleted; }


        public override async Task ExecuteAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            //processamento

            await SaveLogAsync(eventArgs);
        }
    }
}
