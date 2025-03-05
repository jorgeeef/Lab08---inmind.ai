using CourseMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseMicroservice.Data;

public class CourseDbContext : DbContext
{
    public CourseDbContext(DbContextOptions<CourseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
  
}