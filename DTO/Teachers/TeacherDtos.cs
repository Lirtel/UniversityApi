using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Teachers;

public class TeacherDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}

public class TeacherCreateDto
{
    [Required, MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string LastName { get; set; } = string.Empty;
    [Required, MaxLength(150), EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MaxLength(150)] public string Department { get; set; } = string.Empty;
}

public class TeacherUpdateDto : TeacherCreateDto { }