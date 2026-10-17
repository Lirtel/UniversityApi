using System.ComponentModel.DataAnnotations;

namespace UniversityApi.Entities;

public class Teacher
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(150), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Department { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}