using AutoMapper;
using UniversityApi.Common;
using UniversityApi.DTO.Students;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository repo, IMapper mapper, ILogger<StudentService> logger)
    {
        _repo = repo; _mapper = mapper; _logger = logger;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<StudentDto>>(await _repo.GetAllAsync());

    public async Task<StudentDto> GetByIdAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s == null)
        {
            _logger.LogWarning("Student {Id} not found", id);
            throw new NotFoundException("Student", id);
        }
        return _mapper.Map<StudentDto>(s);
    }

    public async Task<StudentWithCoursesDto> GetWithCoursesAsync(int id)
    {
        var s = await _repo.GetByIdWithCoursesAsync(id);
        if (s == null)
        {
            _logger.LogWarning("Student {Id} not found (with courses)", id);
            throw new NotFoundException("Student", id);
        }
        return _mapper.Map<StudentWithCoursesDto>(s);
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto dto)
    {
        if (await _repo.EmailExistsAsync(dto.Email))
            throw new ConflictException("STUDENT_EMAIL_EXISTS", "Student with this email already exists");

        var entity = _mapper.Map<Student>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        var created = await _repo.AddAsync(entity);
        _logger.LogInformation("Student {Id} created", created.Id);
        return _mapper.Map<StudentDto>(created);
    }

    public async Task<StudentDto> UpdateAsync(int id, StudentUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
        {
            _logger.LogWarning("Update failed: Student {Id} not found", id);
            throw new NotFoundException("Student", id);
        }

        if (await _repo.EmailExistsAsync(dto.Email, id))
            throw new ConflictException("STUDENT_EMAIL_EXISTS", "Another student already uses this email");

        _mapper.Map(dto, entity);
        await _repo.UpdateAsync(entity);
        _logger.LogInformation("Student {Id} updated", id);
        return _mapper.Map<StudentDto>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
        {
            _logger.LogWarning("Delete failed: Student {Id} not found", id);
            throw new NotFoundException("Student", id);
        }
        await _repo.DeleteAsync(entity);
        _logger.LogInformation("Student {Id} deleted", id);
    }
}