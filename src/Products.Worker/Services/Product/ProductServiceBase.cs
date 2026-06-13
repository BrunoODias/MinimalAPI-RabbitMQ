using Product.Models;
using ProductApi.Data;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

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

        protected ProductLog GetProductLogFromMessage(BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            var result = JsonSerializer.Deserialize<ProductLog>(message);

            if (result == null)
                throw new Exception("Erro ao ler a mensagem do evento");

            return result;
        }

        internal async Task SaveLogAsync(BasicDeliverEventArgs eventArgs)
        {
            var productLog = GetProductLogFromMessage(eventArgs);

            Console.WriteLine("===================================================================");
            Console.WriteLine($"Operação {productLog.Operation} no objeto({productLog.ProductId}).");
            Console.WriteLine($"Objeto antes: {productLog.Productbefore}");
            Console.WriteLine($"Objeto depois: {productLog.ProductAfter}");
            Console.WriteLine("===================================================================");


            DbContext.ProductLogs.Add(productLog);
            await DbContext.SaveChangesAsync();
        }


        public abstract Task ExecuteAsync(object sender, BasicDeliverEventArgs eventArgs);
    }
}
