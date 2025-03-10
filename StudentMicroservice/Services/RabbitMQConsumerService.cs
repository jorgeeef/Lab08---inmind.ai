using System.Text;
using CourseMicroservice.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StudentMicroservice.Services;

public class RabbitMQConsumerService
{
    private readonly string _host;
    private readonly string _queueName;
    private readonly StudentDbContext _dbContext;

    public RabbitMQConsumerService(IConfiguration configuration, StudentDbContext dbContext)
    {
        _host = configuration["RabbitMQ:Host"];
        _queueName = configuration["RabbitMQ:QueueName"];
        _dbContext = dbContext;
    }

    public void ConsumeMessages()
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received: {message}");
            };
            channel.BasicConsume(queue: _queueName,
                autoAck: true,
                consumer: consumer);
        }
    }
}