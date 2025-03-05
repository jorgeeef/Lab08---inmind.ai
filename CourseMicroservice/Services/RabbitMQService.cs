using System.Text;
using RabbitMQ.Client;

namespace CourseMicroservice.Services;

public class RabbitMQService
{
    private readonly string _host;
    private readonly string _queueName;

    public RabbitMQService(IConfiguration configuration)
    {
        _host = configuration["RabbitMQ:Host"];
        _queueName = configuration["RabbitMQ:QueueName"];
    }

    public void PublishMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = _host };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body);
        }
    }
}
