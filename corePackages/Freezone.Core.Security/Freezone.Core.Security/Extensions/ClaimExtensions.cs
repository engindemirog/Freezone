using System.Security.Claims;

namespace Freezone.Core.Security.Extensions;

public static class ClaimExtensions
{
    public static void AddNameIdentifier(this ICollection<Claim> claims, string id)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
    }

    public static void AddEmail(this ICollection<Claim> claims, string email)
    {
        claims.Add(new Claim(ClaimTypes.Email, email));
    }

    public static void AddName(this ICollection<Claim> claims, string name)
    {
        claims.Add(new Claim(ClaimTypes.Name, name));
    }

    public static void AddRoles(this ICollection<Claim> claims, ICollection<string> roles)
    {
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
    }
}