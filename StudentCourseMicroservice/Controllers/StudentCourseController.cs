using Microsoft.AspNetCore.Mvc;
using StudentEnrollementMicroservice.Data;
using StudentEnrollementMicroservice.Models;

namespace StudentEnrollementMicroservice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentCourseController : ControllerBase
{
    private readonly StudentCourseDbContext _context;

    public StudentCourseController(StudentCourseDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult EnrollStudent([FromBody] StudentCourse enrollment)
    {
        _context.StudentCourses.Add(enrollment);
        _context.SaveChanges();
        return Ok(enrollment);
    }
}
