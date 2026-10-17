using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Repositories.Implementations;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _ctx;
    public EnrollmentRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Enrollment>> GetAllAsync() =>
        await _ctx.Enrollments.AsNoTracking()
            .Include(e => e.Student).Include(e => e.Course).ToListAsync();

    public async Task<Enrollment?> GetByIdAsync(int id) =>
        await _ctx.Enrollments.AsNoTracking()
            .Include(e => e.Student).Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id);

    public Task<bool> ExistsAsync(int studentId, int courseId) =>
        _ctx.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

    public async Task<Enrollment> AddAsync(Enrollment e) { await _ctx.Enrollments.AddAsync(e); await _ctx.SaveChangesAsync(); return e; }
    public async Task UpdateAsync(Enrollment e) { _ctx.Enrollments.Update(e); await _ctx.SaveChangesAsync(); }
    public async Task DeleteAsync(Enrollment e) { _ctx.Enrollments.Remove(e); await _ctx.SaveChangesAsync(); }
}