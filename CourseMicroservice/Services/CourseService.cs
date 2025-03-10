using CourseMicroservice.Data;
using CourseMicroservice.Models;

namespace CourseMicroservice.Services;

public class CourseService
{
    private readonly CourseDbContext _dbContext;

    public CourseService(CourseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCourseAsync(Course course)
    {
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
    }
}