using Hei_Hei_Api.Models;

namespace Hei_Hei_Api.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
