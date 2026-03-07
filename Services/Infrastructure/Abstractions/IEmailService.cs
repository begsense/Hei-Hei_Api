namespace Hei_Hei_Api.Services.Infrastructure.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
