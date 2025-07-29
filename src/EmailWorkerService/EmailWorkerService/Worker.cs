using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EmailWorkerService
{
    public class Worker : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "order-created",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var data = JsonSerializer.Deserialize<OrderCreatedMessage>(json);

                var buyerMsg = $"Dear {data.BuyerFullname}, your order has been received. Product: {data.ProductTitle}";
                var sellerMsg = $"Dear {data.SellerFullname}, your product has been ordered. Product: {data.ProductTitle}";

                Console.WriteLine($"{data.BuyerEmail} => {buyerMsg}");
                Console.WriteLine($"{data.SellerEmail} => {sellerMsg}");
            };

            channel.BasicConsume(
                queue: "order-created",
                autoAck: true,
                consumer: consumer
            );

            return Task.CompletedTask;
        }
    }

    public class OrderCreatedMessage
    {
        public string BuyerFullname { get; set; }
        public string BuyerEmail { get; set; }
        public string SellerFullname { get; set; }
        public string SellerEmail { get; set; }
        public string ProductTitle { get; set; }
    }
}
