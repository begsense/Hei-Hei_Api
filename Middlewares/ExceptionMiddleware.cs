
using System.Net;
using Hei_Hei_Api.Exceptions;
using System.Text.Json;

namespace Hei_Hei_Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            KeyNotFoundException ex => (HttpStatusCode.NotFound, ex.Message),

            InvalidOperationException ex => (HttpStatusCode.BadRequest, ex.Message),

            InvalidCredentialsException ex => (HttpStatusCode.Unauthorized, ex.Message),

            EmailNotVerifiedException ex => (HttpStatusCode.Unauthorized, ex.Message),

            UnauthorizedAccessException ex
                => (HttpStatusCode.Forbidden, string.IsNullOrEmpty(ex.Message)
                    ? "You do not have permission to perform this action."
                    : ex.Message),

            ArgumentException ex => (HttpStatusCode.BadRequest, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = JsonSerializer.Serialize(new
        {
            statusCode = context.Response.StatusCode,
            message
        });

        return context.Response.WriteAsync(response);
    }
}
