using AutoMapper;
using UniversityApi.Common;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repo;
    private readonly IStudentRepository _studentRepo;
    private readonly ICourseRepository _courseRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(IEnrollmentRepository repo, IStudentRepository studentRepo,
        ICourseRepository courseRepo, IMapper mapper, ILogger<EnrollmentService> logger)
    { _repo = repo; _studentRepo = studentRepo; _courseRepo = courseRepo; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<EnrollmentDto>>(await _repo.GetAllAsync());

    public async Task<EnrollmentDto> GetByIdAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null)
        {
            _logger.LogWarning("Enrollment {Id} not found", id);
            throw new NotFoundException("Enrollment", id);
        }
        return _mapper.Map<EnrollmentDto>(e);
    }

    public async Task<EnrollmentDto> CreateAsync(EnrollmentCreateDto dto)
    {
        if (!await _studentRepo.ExistsAsync(dto.StudentId))
        {
            _logger.LogWarning("Enroll failed: Student {Id} not found", dto.StudentId);
            throw new NotFoundException("Student", dto.StudentId);
        }
        if (!await _courseRepo.ExistsAsync(dto.CourseId))
        {
            _logger.LogWarning("Enroll failed: Course {Id} not found", dto.CourseId);
            throw new NotFoundException("Course", dto.CourseId);
        }
        if (await _repo.ExistsAsync(dto.StudentId, dto.CourseId))
        {
            _logger.LogWarning("Duplicate enrollment: Student {S} - Course {C}", dto.StudentId, dto.CourseId);
            throw new ConflictException("DUPLICATE_ENROLLMENT", "Student is already enrolled in this course");
        }

        var entity = _mapper.Map<Enrollment>(dto);
        entity.EnrollmentDate = DateTime.UtcNow;
        var created = await _repo.AddAsync(entity);
        _logger.LogInformation("Enrollment {Id} created (Student {S}, Course {C})",
            created.Id, created.StudentId, created.CourseId);

        var reloaded = await _repo.GetByIdAsync(created.Id);
        return _mapper.Map<EnrollmentDto>(reloaded);
    }

    public async Task<EnrollmentDto> UpdateGradeAsync(int id, EnrollmentUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
        {
            _logger.LogWarning("Update grade failed: Enrollment {Id} not found", id);
            throw new NotFoundException("Enrollment", id);
        }
        entity.Grade = dto.Grade;
        await _repo.UpdateAsync(entity);
        _logger.LogInformation("Enrollment {Id} grade updated to {Grade}", id, dto.Grade);

        var reloaded = await _repo.GetByIdAsync(id);
        return _mapper.Map<EnrollmentDto>(reloaded);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
        {
            _logger.LogWarning("Delete failed: Enrollment {Id} not found", id);
            throw new NotFoundException("Enrollment", id);
        }
        await _repo.DeleteAsync(entity);
        _logger.LogInformation("Enrollment {Id} deleted", id);
    }
}