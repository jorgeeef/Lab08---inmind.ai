using System.ComponentModel.DataAnnotations;

namespace CourseMicroservice.Models;

public class Course
{
    public int Id { get; set; }
        
    [Required]
    public string CourseName { get; set; }
        
    [Required]
    public int Credits { get; set; }
        
    public string? Description { get; set; }
}