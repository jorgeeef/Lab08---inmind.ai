using CourseMicroservice.Data;
using Microsoft.AspNetCore.Mvc;
using StudentEnrollementMicroservice.Models;

namespace StudentMicroservice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly StudentDbContext _context;

    public StudentController(StudentDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddStudent([FromBody] Student student)
    {
        _context.students.Add(student);
        _context.SaveChanges();
        return Ok(student);
    }

    [HttpGet]
    public IActionResult GetAllStudents()
    {
        var students = _context.students.ToList();
        return Ok(students);
    }
}
