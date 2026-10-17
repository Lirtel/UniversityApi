using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Students;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;
    public StudentsController(IStudentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ReturnResult<IEnumerable<StudentDto>>.Success(await _service.GetAllAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(ReturnResult<StudentDto>.Success(await _service.GetByIdAsync(id)));

    [HttpGet("{id:int}/courses")]
    public async Task<IActionResult> GetWithCourses(int id)
        => Ok(ReturnResult<StudentWithCoursesDto>.Success(await _service.GetWithCoursesAsync(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ReturnResult<StudentDto>.Success(result, 201));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto)
        => Ok(ReturnResult<StudentDto>.Success(await _service.UpdateAsync(id, dto)));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(ReturnResult<object>.Success(new { message = "Student deleted" }));
    }
}