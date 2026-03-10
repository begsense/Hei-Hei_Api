using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using System.Diagnostics;
using System.Text;

namespace Hei_Hei_Api.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILoggerService loggerService)
    {
        context.Request.EnableBuffering();

        var requestBody = await ReadStreamAsync(context.Request.Body);
        context.Request.Body.Position = 0;

        var originalResponseStream = context.Response.Body;
        using var responseBuffer = new MemoryStream();
        context.Response.Body = responseBuffer;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
            stopwatch.Stop();

            responseBuffer.Position = 0;
            var responseBody = await ReadStreamAsync(responseBuffer);
            responseBuffer.Position = 0;
            await responseBuffer.CopyToAsync(originalResponseStream);

            await loggerService.LogAsync(
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                requestBody,
                responseBody,
                stopwatch.ElapsedMilliseconds
            );
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            await loggerService.LogErrorAsync(
                context.Request.Method,
                context.Request.Path,
                ex
            );

            context.Response.Body = originalResponseStream;
            throw;
        }
        finally
        {
            context.Response.Body = originalResponseStream;
        }
    }

    private static async Task<string> ReadStreamAsync(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }
}