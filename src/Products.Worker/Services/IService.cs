using Product.Api;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Worker.Services
{
    internal interface IService
    {
        internal string ConsumerBind { get; }
        internal Task ExecuteAsync(object sender, BasicDeliverEventArgs eventArgs);
    }
}
