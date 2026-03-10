namespace Hei_Hei_Api.Services.Infrastructure.Abstractions;

public interface ILoggerService
{
    Task LogAsync(string method, string path, int statusCode, string? requestBody, string? responseBody, long elapsedMs);
    Task LogErrorAsync(string method, string path, Exception exception);
    Task UploadTodayLogToS3Async();
}
