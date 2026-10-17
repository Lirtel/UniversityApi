using UniversityApi.Entities;

namespace UniversityApi.Repositories.Interfaces;

public interface ITeacherRepository
{
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(int id);
    Task<Teacher> AddAsync(Teacher t);
    Task UpdateAsync(Teacher t);
    Task DeleteAsync(Teacher t);
    Task<bool> ExistsAsync(int id);
}