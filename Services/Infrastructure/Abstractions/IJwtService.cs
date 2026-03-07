using Hei_Hei_Api.Models;

namespace Hei_Hei_Api.Services.Infrastructure.Abstractions;

public interface IJwtService
{
    string GenerateToken(User user);
}
