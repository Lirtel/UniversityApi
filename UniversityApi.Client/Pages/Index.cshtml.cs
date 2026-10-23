using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using UniversityApi.Client.Models; // Наши модели

namespace UniversityApi.Client.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    // Сюда мы положим курсы, чтобы показать их на странице
    public List<CourseDto> Courses { get; set; } = new();

    // Сюда запишем текст ошибки, если API упадет
    public string? ErrorMessage { get; set; }

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        // 1. Создаем клиента (он уже знает базовый URL из Program.cs)
        var client = _httpClientFactory.CreateClient("UniversityApi");

        try
        {
            var response = await client.GetFromJsonAsync<ReturnResult<List<CourseDto>>>("/api/v1/courses");

            if (response != null && response.IsSuccess)
            {
                Courses = response.Result ?? new List<CourseDto>();
            }
            else
            {
                ErrorMessage = "API вернул ошибку: " + response?.ErrorMessage;
            }
        }
        catch (Exception ex) // Ловим ЛЮБУЮ ошибку
        {
            // Выводим настоящее сообщение об ошибке
            ErrorMessage = "Ошибка подключения: " + ex.Message;
        }
    }
}