namespace UniversityApi.Common;

public class AppException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }
    public AppException(int statusCode, string errorCode, string message) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string entity, object id)
        : base(404, $"{entity.ToUpper()}_NOT_FOUND", $"{entity} with id {id} was not found") { }
}

public class ConflictException : AppException
{
    public ConflictException(string errorCode, string message) : base(409, errorCode, message) { }
}

public class BadRequestException : AppException
{
    public BadRequestException(string errorCode, string message) : base(400, errorCode, message) { }
}