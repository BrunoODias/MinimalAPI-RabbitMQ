using RabbitMQ.Client.Events;
using System.Text;
using Product.Api;
using Products.Worker.Services;
using System.Reflection.Metadata;

namespace Products.Worker
{
    internal class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly RabbitMQConfigure rabbit;

        public Worker(
            IServiceScopeFactory _scopeFactory,
            RabbitMQConfigure _rabbit)
        {
            scopeFactory = _scopeFactory;
            rabbit = _rabbit;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(rabbit.channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var services =
                            scope.ServiceProvider
                                .GetServices<IService>();

                        var service = services.ToList().FirstOrDefault(x => x.ConsumerBind == eventArgs.RoutingKey);

                        if (service == null)
                            throw new Exception($"Serviço {eventArgs.RoutingKey} não encontrado");
                        
                        await service.ExecuteAsync(sender, eventArgs);
                    }


                    // ACK = OK
                    await rabbit.channel.BasicAckAsync(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false
                    );
                }
                catch (Exception ex)
                {
                    //Log

                    // NACK = NOT OK
                    await rabbit.channel.BasicNackAsync(
                        deliveryTag: eventArgs.DeliveryTag,
                        multiple: false,
                        requeue: true
                    );
                }
            };

            await rabbit.channel.BasicConsumeAsync(
                queue: RabbitMQConfigure.ProductQueueName,
                autoAck: false,
                consumer: consumer,
                consumerTag : "ConsumerTag",
                noLocal: false,
                exclusive: false,
                arguments: null
            );

            // Mantém o worker vivo
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}