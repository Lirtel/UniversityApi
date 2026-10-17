using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Enrollments;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int? Grade { get; set; }
}

public class EnrollmentCreateDto
{
    [Required] public int StudentId { get; set; }
    [Required] public int CourseId { get; set; }
}

public class EnrollmentUpdateDto
{
    [Range(0, 100)] public int? Grade { get; set; }
}