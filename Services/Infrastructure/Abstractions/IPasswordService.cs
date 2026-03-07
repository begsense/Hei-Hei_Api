namespace Hei_Hei_Api.Services.Infrastructure.Abstractions;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
