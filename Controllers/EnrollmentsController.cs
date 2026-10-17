using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;
    public EnrollmentsController(IEnrollmentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ReturnResult<IEnumerable<EnrollmentDto>>.Success(await _service.GetAllAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(ReturnResult<EnrollmentDto>.Success(await _service.GetByIdAsync(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnrollmentCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ReturnResult<EnrollmentDto>.Success(result, 201));
    }

    [HttpPut("{id:int}/grade")]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] EnrollmentUpdateDto dto)
        => Ok(ReturnResult<EnrollmentDto>.Success(await _service.UpdateGradeAsync(id, dto)));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(ReturnResult<object>.Success(new { message = "Enrollment deleted" }));
    }
}