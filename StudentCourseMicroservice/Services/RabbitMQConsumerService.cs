using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;

public class RabbitMQConsumerService
{
    private readonly string _host;
    private readonly string _queueName;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQConsumerService(IConfiguration configuration)
    {
        // Reading RabbitMQ host and queue name from configuration
        _host = configuration["RabbitMQ:Host"];
        _queueName = configuration["RabbitMQ:QueueName"];

        // Initialize RabbitMQ connection and channel
        var factory = new ConnectionFactory() { HostName = _host };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare the queue
        _channel.QueueDeclare(queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public void ConsumeMessages()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received message: {message}");

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName,
            autoAck: false,  
            consumer: consumer);

        Console.WriteLine("Waiting for messages...");
    }

    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
}

