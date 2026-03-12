using System.Security.Claims;

namespace Hei_Hei_Api.Helpers;

public static class GetUserHelper
{
    public static int GetUserId(ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id == null)
        {
            throw new UnauthorizedAccessException();
        }

        return int.Parse(id);
    }
}
