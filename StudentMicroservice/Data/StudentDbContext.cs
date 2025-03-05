using Microsoft.EntityFrameworkCore;
using StudentEnrollementMicroservice.Models;

namespace CourseMicroservice.Data;

public class StudentDbContext : DbContext
{
    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> students { get; set; }
}