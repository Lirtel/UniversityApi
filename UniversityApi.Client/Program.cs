var builder = WebApplication.CreateBuilder(args);

// 1. Добавляем сервисы
builder.Services.AddRazorPages();

// 2. Настраиваем HTTP-клиент ДО того, как собрали приложение
builder.Services.AddHttpClient("UniversityApi", client =>
{
    // Жестко задаем адрес API
    client.BaseAddress = new Uri("http://localhost:7005/");
});

// 3. Собираем приложение (ПОСЛЕ всех AddServices)
var app = builder.Build();

// 4. Дальше настраиваем пайплайн (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();