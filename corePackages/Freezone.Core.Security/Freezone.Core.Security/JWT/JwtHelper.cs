using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Freezone.Core.Security.JWT;

public class JwtHelper : ITokenHelper
{
    private readonly TokenOptions _tokenOptions;

    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
    }

    public AccessToken CreateToken(User user, ICollection<OperationClaim> operationClaims)
    {
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

        SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);

        JwtSecurityToken jwt = createJwtSecurityToken(user, operationClaims, signingCredentials);

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken
        {
            Token = token,
            Expiration = _accessTokenExpiration
        };
    }

    public RefreshToken CreateRefreshToken(User user, string ipAddress)
    {
        return new RefreshToken()
        {
            UserId = user.Id,
            Token = generateRandomRefreshToken(),
            CreatedByIp = ipAddress,
            ExpiresDate = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration),
        };
    }

    private JwtSecurityToken createJwtSecurityToken(User user, ICollection<OperationClaim> operationClaims,
                                                    SigningCredentials signingCredentials) 
        => new JwtSecurityToken(
            _tokenOptions.Issuer,
            _tokenOptions.Audience,
            expires: _accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: setClaims(user, operationClaims),
            signingCredentials: signingCredentials
        );

    private IEnumerable<Claim> setClaims(User user, ICollection<OperationClaim> operationClaims)
    {
        List<Claim> claims = new();

        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddRoles(operationClaims.Select(oc => oc.Name).ToArray());

        return claims.ToArray();
    }

    private string generateRandomRefreshToken()
    {
        byte[] bytes = new byte[32];
        using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes); // İlgili byte arrayini şifrelenmiş random değerlerle doldurur.
        return Convert.ToBase64String(bytes); // byte arrayini string'e base64 ile çeviriyoruz.
    }
}