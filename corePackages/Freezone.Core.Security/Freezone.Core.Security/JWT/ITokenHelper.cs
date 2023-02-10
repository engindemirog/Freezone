using Freezone.Core.Security.Entities;

namespace Freezone.Core.Security.JWT;

public interface ITokenHelper
{
    public int RefreshTokenTTLOption { get; }
    public AccessToken CreateToken(User user, ICollection<OperationClaim> operationClaim);
    public RefreshToken CreateRefreshToken(User user, string ipAddress);
}