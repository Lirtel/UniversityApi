using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Teachers;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _service;
    public TeachersController(ITeacherService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ReturnResult<IEnumerable<TeacherDto>>.Success(await _service.GetAllAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(ReturnResult<TeacherDto>.Success(await _service.GetByIdAsync(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeacherCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ReturnResult<TeacherDto>.Success(result, 201));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TeacherUpdateDto dto)
        => Ok(ReturnResult<TeacherDto>.Success(await _service.UpdateAsync(id, dto)));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(ReturnResult<object>.Success(new { message = "Teacher deleted" }));
    }
}