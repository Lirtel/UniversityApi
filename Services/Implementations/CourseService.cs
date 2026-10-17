using AutoMapper;
using UniversityApi.Common;
using UniversityApi.DTO.Courses;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repo;
    private readonly ITeacherRepository _teacherRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<CourseService> _logger;

    public CourseService(ICourseRepository repo, ITeacherRepository teacherRepo,
        IMapper mapper, ILogger<CourseService> logger)
    { _repo = repo; _teacherRepo = teacherRepo; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<CourseDto>> GetAllAsync(string? search, int? teacherId, int? minCredits) =>
        _mapper.Map<IEnumerable<CourseDto>>(await _repo.GetAllAsync(search, teacherId, minCredits));

    public async Task<CourseDto> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null)
        {
            _logger.LogWarning("Course {Id} not found", id);
            throw new NotFoundException("Course", id);
        }
        return _mapper.Map<CourseDto>(c);
    }

    public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
    {
        if (!await _teacherRepo.ExistsAsync(dto.TeacherId))
        {
            _logger.LogWarning("Create course failed: Teacher {Id} not found", dto.TeacherId);
            throw new NotFoundException("Teacher", dto.TeacherId);
        }

        var entity = _mapper.Map<Course>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        var created = await _repo.AddAsync(entity);
        _logger.LogInformation("Course {Id} created", created.Id);

        var reloaded = await _repo.GetByIdAsync(created.Id);
        return _mapper.Map<CourseDto>(reloaded);
    }

    public async Task<CourseDto> UpdateAsync(int id, CourseUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
        {
            _logger.LogWarning("Update failed: Course {Id} not found", id);
            throw new NotFoundException("Course", id);
        }

        if (!await _teacherRepo.ExistsAsync(dto.TeacherId))
            throw new NotFoundException("Teacher", dto.TeacherId);

        _mapper.Map(dto, entity);
        await _repo.UpdateAsync(entity);
        _logger.LogInformation("Course {Id} updated", id);

        var reloaded = await _repo.GetByIdAsync(id);
        return _mapper.Map<CourseDto>(reloaded);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
        {
            _logger.LogWarning("Delete failed: Course {Id} not found", id);
            throw new NotFoundException("Course", id);
        }
        await _repo.DeleteAsync(entity);
        _logger.LogInformation("Course {Id} deleted", id);
    }
}