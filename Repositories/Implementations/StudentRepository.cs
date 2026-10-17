using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Repositories.Implementations;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _ctx;
    public StudentRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Student>> GetAllAsync() =>
        await _ctx.Students.AsNoTracking().ToListAsync();

    public async Task<Student?> GetByIdAsync(int id) =>
        await _ctx.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Student?> GetByIdWithCoursesAsync(int id) =>
        await _ctx.Students.AsNoTracking()
            .Include(s => s.Enrollments).ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Student> AddAsync(Student student)
    {
        await _ctx.Students.AddAsync(student);
        await _ctx.SaveChangesAsync();
        return student;
    }

    public async Task UpdateAsync(Student student)
    {
        _ctx.Students.Update(student);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(Student student)
    {
        _ctx.Students.Remove(student);
        await _ctx.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(int id) => _ctx.Students.AnyAsync(s => s.Id == id);

    public Task<bool> EmailExistsAsync(string email, int? excludeId = null) =>
        _ctx.Students.AnyAsync(s => s.Email == email && (excludeId == null || s.Id != excludeId));
}