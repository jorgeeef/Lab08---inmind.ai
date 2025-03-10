using System.Text;
using CourseMicroservice.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CourseMicroservice.Services;

public class RabbitMQService: BackgroundService
{
    private readonly IServiceProvider _serviceProvider; 

    public RabbitMQService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest1",
            Password = "guest1"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "CourseExchange", type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);

        channel.QueueDeclare(queue: "CourseQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        channel.QueueBind(queue: "CourseQueue", exchange: "CourseExchange", routingKey: "course.create");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var courseService = scope.ServiceProvider.GetRequiredService<CourseService>();

                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var course = Newtonsoft.Json.JsonConvert.DeserializeObject<Course>(message);

                    await courseService.CreateCourseAsync(course);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing message: {ex.Message}");
                    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            }
        };

        channel.BasicConsume(queue: "CourseQueue", autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
    
}
