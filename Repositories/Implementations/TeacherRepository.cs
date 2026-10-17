using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Repositories.Implementations;

public class TeacherRepository : ITeacherRepository
{
    private readonly ApplicationDbContext _ctx;
    public TeacherRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Teacher>> GetAllAsync() =>
        await _ctx.Teachers.AsNoTracking().ToListAsync();

    public async Task<Teacher?> GetByIdAsync(int id) =>
        await _ctx.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Teacher> AddAsync(Teacher t) { await _ctx.Teachers.AddAsync(t); await _ctx.SaveChangesAsync(); return t; }
    public async Task UpdateAsync(Teacher t) { _ctx.Teachers.Update(t); await _ctx.SaveChangesAsync(); }
    public async Task DeleteAsync(Teacher t) { _ctx.Teachers.Remove(t); await _ctx.SaveChangesAsync(); }
    public Task<bool> ExistsAsync(int id) => _ctx.Teachers.AnyAsync(t => t.Id == id);
}