using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using System.Net.Mail;

namespace Hei_Hei_Api.Services.Infrastructure.Implementations;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    private readonly string email;
    private readonly string emailPassword;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;

        email = _configuration["EmailSettings:Email"];
        emailPassword = _configuration["EmailSettings:Password"];
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var mailMessage = new MailMessage(email, to, subject, body);
        mailMessage.IsBodyHtml = true;

        using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new System.Net.NetworkCredential(email, emailPassword),
            EnableSsl = true
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}