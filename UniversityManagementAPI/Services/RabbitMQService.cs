using System.Text;
using RabbitMQ.Client;

namespace UniversityManagementAPI.Services;

public class RabbitMQService
{
    private readonly string _host;
    private readonly string _courseQueue;
    private readonly string _studentCourseQueue;

    public RabbitMQService(IConfiguration configuration)
    {
        _host = configuration["RabbitMQ:Host"];
        _courseQueue = configuration["RabbitMQ:CourseQueue"];
        _studentCourseQueue = configuration["RabbitMQ:StudentCourseQueue"];
    }

    public void PublishToCourseQueue(string message)
    {
        var factory = new ConnectionFactory() { HostName = _host };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _courseQueue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: _courseQueue,
                                 basicProperties: null,
                                 body: body);
        }
    }

    public void PublishToStudentCourseQueue(string message)
    {
        var factory = new ConnectionFactory() { HostName = _host };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _studentCourseQueue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: _studentCourseQueue,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
