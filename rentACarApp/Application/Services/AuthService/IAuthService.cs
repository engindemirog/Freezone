using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;

namespace Application.Services.AuthService;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessToken(User user);
    
    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
    public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
    public Task DeleteOldActiveRefreshTokens(User user); // Login olunduğunda eski aktif refresh token'ları (TTL süresiyle birlikte) siler

    public Task RevokeRefreshToken(RefreshToken token, string ipAddress, string reason, string? replacedByToken = null); // Bir refresh token'ı geçersiz kılar
    public Task RevokeDescendantRefreshTokens(RefreshToken token, string ipAddress, string reason); // Bir refresh token'ı geçersiz kılar

    public Task<RefreshToken> RotateRefreshToken(User user, RefreshToken token, string ipAddress); // Önceki refresh token'ı geçersiz kılıcak (revoke) aktifliği yeni refresh token'a vericek.

}