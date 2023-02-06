using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;

namespace Application.Services.AuthService;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessToken(User user);
}