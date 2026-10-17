# University Course Management API

REST Web API для системы управления университетскими курсами, разработанный на ASP.NET Core 8.

## Стек технологий
* ASP.NET Core Web API
* Entity Framework Core (SQL Server)
* AutoMapper
* Swagger / OpenAPI

## Архитектура проекта
Проект разделен на слои:
`Client` → `Controller` → `Service` → `Repository` → `EF Core` → `Database`

## Как запустить
1. Убедитесь, что у вас установлен .NET 8 SDK и SQL Server (LocalDB).
2. Клонируйте репозиторий.
3. Откройте проект в Visual Studio.
4. Запустите приложение (F5). База данных создастся автоматически (используется `EnsureCreated`).
5. Перейдите в Swagger по адресу: `https://localhost:<порт>/swagger`

## Основные Endpoints
* `GET /api/students` — список всех студентов
* `POST /api/students` — создание студента
* `GET /api/students/{id}/courses` — студент вместе с его курсами
* `POST /api/enrollments` — запись студента на курс
* `PUT /api/enrollments/{id}/grade` — изменение оценки
* `GET /api/courses?search=React&minCredits=4` — фильтрация курсов

*(Полный список доступен в Swagger UI).*

## Формат ответа
Все ответы API унифицированы:
```json
{
  "statusCode": 200,
  "isSuccess": true,
  "result": { ... },
  "errorCode": null,
  "errorMessage": null,
  "traceId": "..."
}

```markdown
## Схема архитектуры

```mermaid
graph TD
    Client[Client / Postman / Swagger] -->|HTTP Requests| API[Controllers]
    API -->|DTOs| Service[Services / Business Logic]
    Service -->|Entities| Repo[Repositories]
    Repo -->|LINQ| EF[Entity Framework Core]
    EF -->|SQL| DB[(SQL Server)]

