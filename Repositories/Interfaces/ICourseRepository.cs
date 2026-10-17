using UniversityApi.Entities;

namespace UniversityApi.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync(string? search, int? teacherId, int? minCredits);
    Task<Course?> GetByIdAsync(int id);
    Task<Course> AddAsync(Course c);
    Task UpdateAsync(Course c);
    Task DeleteAsync(Course c);
    Task<bool> ExistsAsync(int id);
}