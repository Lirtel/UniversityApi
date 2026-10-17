using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Courses;

public class CourseBriefDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CourseCreateDto
{
    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(1000)] public string? Description { get; set; }
    [Range(1, 10)] public int Credits { get; set; }
    [Required] public int TeacherId { get; set; }
}

public class CourseUpdateDto : CourseCreateDto { }