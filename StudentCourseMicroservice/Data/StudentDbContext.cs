using Microsoft.EntityFrameworkCore;
using StudentEnrollementMicroservice.Models;

namespace StudentEnrollementMicroservice.Data;

public class StudentCourseDbContext : DbContext
{
    public StudentCourseDbContext(DbContextOptions<StudentCourseDbContext> options) : base(options) { }

    public DbSet<StudentCourse> StudentCourses { get; set; }
}