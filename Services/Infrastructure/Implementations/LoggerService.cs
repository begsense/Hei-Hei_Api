using Hei_Hei_Api.Services.Infrastructure.Abstractions;

namespace Hei_Hei_Api.Services.Infrastructure.Implementations;

public class LoggerService : ILoggerService
{
    private readonly string _logDirectory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly IS3Service _s3Service;

    public LoggerService(IConfiguration configuration, IS3Service s3Service)
    {
        _s3Service = s3Service;

        _logDirectory = configuration["Logging:LogDirectory"] ?? "Logs";
        Directory.CreateDirectory(_logDirectory);
    }

    public async Task LogAsync(string method, string path, int statusCode, string? requestBody, string? responseBody, long elapsedMs)
    {
        var entry = $"""
            ────────────────────────────────────────
            [{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {method} {path}
            Status   : {statusCode} {GetStatusLabel(statusCode)}
            Duration : {elapsedMs}ms
            Request  : {Sanitize(requestBody)}
            Response : {Sanitize(responseBody)}
            """;

        await WriteAsync(entry);
    }

    public async Task LogErrorAsync(string method, string path, Exception exception)
    {
        var entry = $"""
            ════════════════════════════════════════
            [{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] ERROR {method} {path}
            Message  : {exception.Message}
            Type     : {exception.GetType().Name}
            Stack    :
            {exception.StackTrace}
            ════════════════════════════════════════
            """;

        await WriteAsync(entry);
    }

    public async Task UploadTodayLogToS3Async()
    {
        var fileName = $"{DateTime.UtcNow:yyyy-MM-dd}.log";
        var filePath = Path.Combine(_logDirectory, fileName);

        if (!File.Exists(filePath))
            return;

        await using var stream = File.OpenRead(filePath);

        await _s3Service.UploadPrivateFileAsync(stream, $"logs/{fileName}", "text/plain");
    }

    private async Task WriteAsync(string entry)
    {
        var filePath = Path.Combine(_logDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.log");

        await _semaphore.WaitAsync();
        try
        {
            await File.AppendAllTextAsync(filePath, entry + Environment.NewLine);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static string GetStatusLabel(int statusCode) => statusCode switch
    {
        >= 200 and < 300 => "✓ Success",
        >= 400 and < 500 => "✗ Client Error",
        >= 500 => "✗ Server Error",
        _ => ""
    };

    private static string Sanitize(string? body)
    {
        if (string.IsNullOrWhiteSpace(body)) return "—";

        return System.Text.RegularExpressions.Regex.Replace(
            body,
            @"""(password|passwordHash|currentPassword|newPassword|confirmNewPassword)""\s*:\s*""[^""]*""",
            @"""$1"": ""[REDACTED]""",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );
    }
}