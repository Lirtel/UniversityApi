using Microsoft.AspNetCore.Authorization;
using UniversityApi.Entities;

namespace UniversityApi.Authorization;

public class CourseOwnerRequirement : IAuthorizationRequirement { }

public class CourseOwnerHandler : AuthorizationHandler<CourseOwnerRequirement, Course>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CourseOwnerRequirement requirement, Course resource)
    {
        // Если пользователь Админ — разрешаем всё
        if (context.User.IsInRole("Admin")) { context.Succeed(requirement); return Task.CompletedTask; }

        // Получаем TeacherId из Claims
        var teacherIdClaim = context.User.FindFirst("TeacherId")?.Value;
        if (int.TryParse(teacherIdClaim, out int teacherId) && resource.TeacherId == teacherId)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}