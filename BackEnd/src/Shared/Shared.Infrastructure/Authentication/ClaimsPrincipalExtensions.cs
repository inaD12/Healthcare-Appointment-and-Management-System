using System.Security.Claims;
using Shared.Utilities;

namespace Shared.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirst(AppClaims.Sub)?.Value;

        if (userId == null)
            throw new Exception("User identifier is unavailable");
        
        return userId;
    }

    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new Exception("User identity is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        IEnumerable<Claim> permissionClaims = principal?.FindAll(AppClaims.Permission) ??
                                              throw new Exception("Permissions are unavailable");

        return permissionClaims.Select(c => c.Value).ToHashSet();
    }
}
