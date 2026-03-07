using BCrypt.Net;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;

namespace Hei_Hei_Api.Services.Infrastructure.Implementations;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
