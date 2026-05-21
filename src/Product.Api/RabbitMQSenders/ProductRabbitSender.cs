using Microsoft.AspNetCore.Http;
using Product.Models;
using RabbitMQ.Client;
using System.Text.Json;

namespace Product.Api
{
    public class ProductRabbitSender
    {
        private RabbitMQConfigure rabbit { get; set; }
        public ProductRabbitSender(RabbitMQConfigure _rabbit)
        {
            rabbit = _rabbit;
        }

        private ProductLog CreateProductEventLog(HttpContext httpContext, ProductModel productBefore, string Operation, ProductModel? productAfter = null)
        {
            string userIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? httpContext.Request.Headers["X-Forwarded-For"].ToString();
            return new ProductLog(productBefore, userIp, Operation, DateTime.Now, productAfter);
        }

        public async Task SendMessageAsnyc(HttpContext httpContext, ProductModel productBefore, string Operation, ProductModel? productAfter = null)
        {

            //Envio da mensagem de produto criado
            byte[] bodyBytes = JsonSerializer.SerializeToUtf8Bytes(
                CreateProductEventLog(
                    httpContext,
                    productBefore,
                    RabbitMQConfigure.ProductEventsRoutingKey.Created,
                    productAfter
                )
            );

            await rabbit.channel.BasicPublishAsync(
                exchange: RabbitMQConfigure.ProductExchangeName,
                routingKey: Operation,
                mandatory: false,
                basicProperties: new BasicProperties()
                {
                    Persistent = true,
                    ContentType = "application/json",
                    ContentEncoding = "utf-8"
                },
                body: bodyBytes
            );
        }
    }
}
