using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Repositories.Implementations;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _ctx;
    public CourseRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Course>> GetAllAsync(string? search, int? teacherId, int? minCredits)
    {
        var q = _ctx.Courses.AsNoTracking().Include(c => c.Teacher).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(c => c.Name.Contains(search));
        if (teacherId.HasValue) q = q.Where(c => c.TeacherId == teacherId.Value);
        if (minCredits.HasValue) q = q.Where(c => c.Credits >= minCredits.Value);
        return await q.ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id) =>
        await _ctx.Courses.AsNoTracking().Include(c => c.Teacher).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Course> AddAsync(Course c) { await _ctx.Courses.AddAsync(c); await _ctx.SaveChangesAsync(); return c; }
    public async Task UpdateAsync(Course c) { _ctx.Courses.Update(c); await _ctx.SaveChangesAsync(); }
    public async Task DeleteAsync(Course c) { _ctx.Courses.Remove(c); await _ctx.SaveChangesAsync(); }
    public Task<bool> ExistsAsync(int id) => _ctx.Courses.AnyAsync(c => c.Id == id);
}