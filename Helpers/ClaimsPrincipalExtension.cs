using System.Security.Claims;

namespace Hei_Hei_Api.Helpers;

public static class ClaimsPrincipalExtension
{
    public static bool IsAdminOrOwner(this ClaimsPrincipal user, int resourceOwnerId)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return id != null && (user.IsInRole("Admin") || id == resourceOwnerId.ToString());
    }
}
