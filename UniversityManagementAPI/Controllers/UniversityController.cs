using Microsoft.AspNetCore.Mvc;
using UniversityManagementAPI.Models;
using UniversityManagementAPI.Services;

namespace UniversityManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UniversityController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RabbitMQService _rabbitMQService;
    private readonly IConfiguration _configuration;

    public UniversityController(IHttpClientFactory httpClientFactory, RabbitMQService rabbitMQService, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _rabbitMQService = rabbitMQService;
        _configuration = configuration;
    }

    [HttpPost("addStudent")]
    public async Task<IActionResult> AddStudent([FromBody] Student student)
    {
        // Call the Student Microservice to add the student
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(_configuration["Microservices:StudentServiceUrl"], student);
        
        if (response.IsSuccessStatusCode)
        {
            // Send a message to the StudentCourseMicroservice (via RabbitMQ)
            _rabbitMQService.PublishToStudentCourseQueue($"New student enrolled: {student.FirstName} {student.LastName}");

            return Ok("Student added successfully.");
        }

        return BadRequest("Failed to add student.");
    }
    
    [HttpPost("addCourse")]
    public async Task<IActionResult> AddCourse([FromBody] Course course)
    {
        // Call the Course Microservice to add the course
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(_configuration["Microservices:CourseServiceUrl"], course);
    
        if (response.IsSuccessStatusCode)
        {
            // Send a message to the StudentCourseMicroservice (via RabbitMQ)
            _rabbitMQService.PublishToStudentCourseQueue($"New course added: {course.CourseName}");

            return Ok("Course added successfully.");
        }

        return BadRequest("Failed to add course.");
    }
    
    [HttpPost("enrollStudent")]
    public async Task<IActionResult> EnrollStudent([FromBody] StudentCourse enrollRequest)
    {
        // Call the StudentCourse Microservice to handle the enrollment
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(_configuration["Microservices:StudentCourseServiceUrl"], enrollRequest);
    
        if (response.IsSuccessStatusCode)
        {
            // Publish to RabbitMQ to notify enrollment
            _rabbitMQService.PublishToStudentCourseQueue($"Student enrolled in course: {enrollRequest.StudentId}, {enrollRequest.CourseId}");

            return Ok("Student enrolled in the course.");
        }

        return BadRequest("Failed to enroll student.");
    }

}
