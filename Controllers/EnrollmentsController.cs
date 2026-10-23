using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Repositories.Interfaces;
using UniversityApi.Services.Interfaces;

namespace UniversityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;
    private readonly ICourseRepository _courseRepo; // Добавили для проверки владельца курса
    private readonly IAuthorizationService _authorizationService; // Добавили для политики

    // Обновили конструктор, чтобы внедрить новые зависимости
    public EnrollmentsController(
        IEnrollmentService service,
        ICourseRepository courseRepo,
        IAuthorizationService authorizationService)
    {
        _service = service;
        _courseRepo = courseRepo;
        _authorizationService = authorizationService;
    }

    // Получать записи может только авторизованный пользователь
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
        => Ok(ReturnResult<IEnumerable<EnrollmentDto>>.Success(await _service.GetAllAsync()));

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
        => Ok(ReturnResult<EnrollmentDto>.Success(await _service.GetByIdAsync(id)));

    // Записывать на курс может Админ или Студент (но лучше ограничить логику в сервисе)
    [HttpPost]
    [Authorize(Roles = "Admin,Student")]
    public async Task<IActionResult> Create([FromBody] EnrollmentCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ReturnResult<EnrollmentDto>.Success(result, 201));
    }

    // ОСТАВЛЯЕМ ТОЛЬКО ОДИН МЕТОД UpdateGrade С ПРОВЕРКОЙ ПРАВ
    [HttpPut("{id:int}/grade")]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] EnrollmentUpdateDto dto)
    {
        // 1. Получаем запись, чтобы узнать ID курса
        var enrollment = await _service.GetByIdAsync(id);

        // 2. Загружаем курс, чтобы проверить владельца
        var course = await _courseRepo.GetByIdAsync(enrollment.CourseId);
        if (course == null) return NotFound();

        // 3. Проверяем политику "CourseOwner" (админ пройдет, учитель - только если это его курс)
        var authResult = await _authorizationService.AuthorizeAsync(User, course, "CourseOwner");
        if (!authResult.Succeeded)
            return Forbid(); // 403 Forbidden

        // 4. Если права есть — обновляем оценку
        var updated = await _service.UpdateGradeAsync(id, dto);
        return Ok(ReturnResult<EnrollmentDto>.Success(updated));
    }

    // Удалять записи может только Админ
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(ReturnResult<object>.Success(new { message = "Enrollment deleted" }));
    }
}