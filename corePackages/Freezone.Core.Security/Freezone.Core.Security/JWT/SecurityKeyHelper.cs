using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Freezone.Core.Security.JWT;

public static class SecurityKeyHelper
{
    public static SecurityKey CreateSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}