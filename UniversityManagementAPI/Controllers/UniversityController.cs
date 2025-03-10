using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using UniversityManagementAPI.Models;

namespace UniversityManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UniversityController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public UniversityController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpPost("addCourse")]
    public async Task<IActionResult> AddCourse([FromBody] Course course)
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

        var message = Newtonsoft.Json.JsonConvert.SerializeObject(course);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "CourseExchange", routingKey: "course.create", basicProperties: null, body: body);

        return Ok("Course creation request sent to CourseManagement.");
    }
    
    

}
