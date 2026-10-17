using AutoMapper;
using UniversityApi.Common;
using UniversityApi.DTO.Teachers;
using UniversityApi.Entities;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Services.Implementations;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<TeacherService> _logger;

    public TeacherService(ITeacherRepository repo, IMapper mapper, ILogger<TeacherService> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<IEnumerable<TeacherDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<TeacherDto>>(await _repo.GetAllAsync());

    public async Task<TeacherDto> GetByIdAsync(int id)
    {
        var t = await _repo.GetByIdAsync(id) ?? throw NotFound(id);
        return _mapper.Map<TeacherDto>(t);
    }

    public async Task<TeacherDto> CreateAsync(TeacherCreateDto dto)
    {
        var entity = _mapper.Map<Teacher>(dto);
        var created = await _repo.AddAsync(entity);
        _logger.LogInformation("Teacher {Id} created", created.Id);
        return _mapper.Map<TeacherDto>(created);
    }

    public async Task<TeacherDto> UpdateAsync(int id, TeacherUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id) ?? throw NotFound(id);
        _mapper.Map(dto, entity);
        await _repo.UpdateAsync(entity);
        _logger.LogInformation("Teacher {Id} updated", id);
        return _mapper.Map<TeacherDto>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id) ?? throw NotFound(id);
        await _repo.DeleteAsync(entity);
        _logger.LogInformation("Teacher {Id} deleted", id);
    }

    private NotFoundException NotFound(int id)
    {
        _logger.LogWarning("Teacher {Id} not found", id);
        return new NotFoundException("Teacher", id);
    }
}