using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace E_CommerceAPI.Infrastructure.Messaging;

public class RabbitMQPublisher
{
    public void PublishOrderCreated(OrderCreatedMessage message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "order-created", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: "order-created", basicProperties: null, body: body);
    }
}

public class OrderCreatedMessage
{
    public string BuyerEmail { get; set; }
    public string SellerEmail { get; set; }
    public string BuyerFullname { get; set; }
    public string SellerFullname { get; set; }
    public string ProductTitle { get; set; }
}
