using System.Diagnostics;

namespace UniversityApi.Common;

public class ReturnResult<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public T? Result { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? TraceId { get; set; }

    public static ReturnResult<T> Success(T result, int statusCode = 200) => new()
    {
        StatusCode = statusCode,
        IsSuccess = true,
        Result = result,
        TraceId = Activity.Current?.Id ?? Guid.NewGuid().ToString("N")
    };

    public static ReturnResult<T> Failure(int statusCode, string errorCode, string errorMessage) => new()
    {
        StatusCode = statusCode,
        IsSuccess = false,
        Result = default,
        ErrorCode = errorCode,
        ErrorMessage = errorMessage,
        TraceId = Activity.Current?.Id ?? Guid.NewGuid().ToString("N")
    };
}