using UniversityApi.DTO.Courses;

namespace UniversityApi.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync(string? search, int? teacherId, int? minCredits);
    Task<CourseDto> GetByIdAsync(int id);
    Task<CourseDto> CreateAsync(CourseCreateDto dto);
    Task<CourseDto> UpdateAsync(int id, CourseUpdateDto dto);
    Task DeleteAsync(int id);
}