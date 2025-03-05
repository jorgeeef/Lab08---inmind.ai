using Microsoft.EntityFrameworkCore;
using UniversityManagementAPI.Models;

namespace UniversityManagementAPI.Data
{
    public class UniversityDbContext : DbContext
    {
        private readonly string _tenantSchema;

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options, string tenantSchema)
            : base(options)
        {
            _tenantSchema = tenantSchema;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dynamically set the schema for each table
            modelBuilder.Entity<Student>().ToTable("Students", _tenantSchema);
            modelBuilder.Entity<Course>().ToTable("Courses", _tenantSchema);
            modelBuilder.Entity<StudentCourse>().ToTable("Enrollments", _tenantSchema);
        }

        // Define DbSets for your entities
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> Enrollments { get; set; }
    }
}