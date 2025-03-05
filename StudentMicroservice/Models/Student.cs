using System.ComponentModel.DataAnnotations;

namespace StudentEnrollementMicroservice.Models;

public class Student
{
    public int Id { get; set; }
        
    [Required]
    public string FirstName { get; set; }
        
    [Required]
    public string LastName { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public int Age { get; set; }
}