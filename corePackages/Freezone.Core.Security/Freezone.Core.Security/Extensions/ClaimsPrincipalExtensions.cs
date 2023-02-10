using System.Security.Claims;

namespace Freezone.Core.Security.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal?.FindAll(claimType)?.Select(c => c.Value).ToList();
    }

    public static List<string>? ClaimsRoles(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims(ClaimTypes.Role);
    }

    public static int ClaimsNameIdentifier(this ClaimsPrincipal claimsPrincipal)
    {
        return Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier).FirstOrDefault());
    }
}