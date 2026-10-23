using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Courses;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class CoursesV2Controller : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesV2Controller(ICourseService courseService) => _courseService = courseService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Используем ту же бизнес-логику, но новый DTO
        var courses = await _courseService.GetAllAsync(null, null, null);
        var result = courses.Select(c => new CourseV2Dto
        {
            Id = c.Id,
            Name = c.Name,
            Credits = c.Credits,
            TeacherName = c.TeacherName,
            EnrolledStudentsCount = 42 // В реальности: запрос к EnrollmentRepository
        });
        return Ok(ReturnResult<IEnumerable<CourseV2Dto>>.Success(result));
    }
}