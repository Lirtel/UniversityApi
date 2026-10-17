using UniversityApi.Entities;

namespace UniversityApi.Repositories.Interfaces;

public interface IEnrollmentRepository
{
    Task<IEnumerable<Enrollment>> GetAllAsync();
    Task<Enrollment?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int studentId, int courseId);
    Task<Enrollment> AddAsync(Enrollment e);
    Task UpdateAsync(Enrollment e);
    Task DeleteAsync(Enrollment e);
}