using CourseMicroservice.Data;
using CourseMicroservice.Models;
using CourseMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseMicroservice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly CourseDbContext _context;
    private readonly RabbitMQService _rabbitMQService;

    public CourseController(CourseDbContext context, RabbitMQService rabbitMQService)
    {
        _context = context;
        _rabbitMQService = rabbitMQService;
    }

    [HttpPost]
    public IActionResult AddCourse([FromBody] Course course)
    {
        _context.Courses.Add(course);
        _context.SaveChanges();
        _rabbitMQService.PublishMessage($"New course added: {course.CourseName}");
        return Ok(course);
    }

    [HttpGet]
    public IActionResult GetAllCourses()
    {
        var courses = _context.Courses.ToList();
        return Ok(courses);
    }
}
