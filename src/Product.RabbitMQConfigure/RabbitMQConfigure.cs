using RabbitMQ.Client;
using System.Text.Json;

namespace Product.Api
{
    public class RabbitMQConfigure
    {
        private readonly IConnection _connection;
        public IChannel channel { get; }

        public RabbitMQConfigure()
        {
            var factory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = HostName,
                Port = Port,
                UserName = UserName,
                Password = Password,
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnectionAsync().Result;
            channel = _connection.CreateChannelAsync().Result;

            Configure().Wait();
        }

        //Arquivo de config
        const string HostName = "localhost";
        const int Port = 5672;
        const string UserName = "guest";
        const string Password = "guest";


        public const string ProductExchangeName = "product.exchange";
        public const string ProductQueueName = "product.queue";
        public const string ProductRoutingKey = "product.*";

        public static class ProductEventsRoutingKey
        {
            public static readonly string Created = "product.created";
            public static readonly string Edited = "product.edited";
            public static readonly string Deleted = "product.deleted";
        };



        public async Task Configure()
        {

            await channel.ExchangeDeclareAsync(
                exchange: ProductExchangeName,
                type: RabbitMQ.Client.ExchangeType.Topic,
                durable: true,
                autoDelete: false
            );

            await channel.QueueDeclareAsync(
                queue: ProductQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: ProductQueueName,
                exchange: ProductExchangeName,
                routingKey: ProductRoutingKey
            );

            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false
            );
        }
    }
}
