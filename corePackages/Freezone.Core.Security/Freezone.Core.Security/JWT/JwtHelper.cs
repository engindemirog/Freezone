using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Freezone.Core.Security.JWT;

public class JwtHelper: ITokenHelper
{
    private TokenOptions _tokenOptions;
    
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
    }

    public AccessToken CreateToken(User user, ICollection<OperationClaim> operationClaims)
    {
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);

        var jwt = createJwtSecurityToken(user, operationClaims, signingCredentials);

        //TODO

        return new AccessToken();
    }

    private JwtSecurityToken createJwtSecurityToken(User user, ICollection<OperationClaim> operationClaims, SigningCredentials signingCredentials)
    {
        return new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            expires: _accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: setClaims(user, operationClaims),
            signingCredentials: signingCredentials
            );
    }

    private IEnumerable<Claim> setClaims(User user, ICollection<OperationClaim> operationClaims)
    {
        List<Claim> claims = new();
        
        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddRoles(operationClaims.Select(oc => oc.Name).ToArray());

        return claims;
    }
}