using Application.Services.Repositories;
using Freezone.Core.CrossCuttingConcerns.Exceptions;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.AuthService;

public class AuthService : IAuthService
{
    private IUserOperationClaimRepository _userOperationClaimRepository;
    private ITokenHelper _tokenHelper;
    IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
        _tokenHelper = tokenHelper;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AccessToken> CreateAccessToken(User user)
    {
        var operationClaims = await _userOperationClaimRepository.GetOperationClaimsByUserIdAsync(user.Id);
        var accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }

    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }

    public async Task DeleteOldActiveRefreshTokens(User user)
    {
        // Yeni login işleminde, yeni bir refresh token zinciri oluşacaktır. Hali hazırda bulunan diğer zincirler (en son aktif olan token'a göre) yeterince eskiyse (RefreshTokenTTL opsiyonundaki süre kadar) silinir.
        ICollection<RefreshToken> oldActiveRefreshTokens = await _refreshTokenRepository
                                                                   .GetAllOldActiveRefreshTokensAsync(
                                                                       user, _tokenHelper.RefreshTokenTTLOption);
        await _refreshTokenRepository.DeleteRangeAsync(oldActiveRefreshTokens.ToList());
    }

    public async Task RevokeRefreshToken(RefreshToken token, string ipAddress, string reason, string? replacedByToken = null)
    {
        // Bir refresh token'ı geçersiz kılmak için ilgili bilgileri doldurur ve veri tabanında günceller.
        token.RevokedDate = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.RevokedReason = reason;
        token.ReplacedByToken = replacedByToken;
        await _refreshTokenRepository.UpdateAsync(token);
    }

    public async Task RevokeDescendantRefreshTokens(RefreshToken token, string ipAddress, string reason)
    {
        // Eğerki hali hazırda geçersiz refresh token kullanılmaya çalışılırsa, o refresh token'ın tüm child refresh token'ları gezilerek en son zincirdeki aktif refresh token geçersiz kılınır.
        RefreshToken childRefreshToken =
            (await _refreshTokenRepository.GetAsync(rt => token.ReplacedByToken == rt.Token))!;
        if (childRefreshToken == null) throw new BusinessException("Kullanılmaya çalışılan Refresh Token'ın child bulunamadı");

        if (childRefreshToken.RevokedDate == null) await RevokeRefreshToken(childRefreshToken, ipAddress, reason);
        else await RevokeDescendantRefreshTokens(childRefreshToken, ipAddress, reason);
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken token, string ipAddress)
    {
        // Kullanılan refresh token'ı geçersiz kılıcak (revoke) aktifliği yeni refresh token'a vericek. Refresh token'larla oluşan oturum temsil eden zincire yeni aktif bir refresh token eklenmiş olacak.
        RefreshToken newRefreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        
        await RevokeRefreshToken(token, ipAddress, "Yeni Refresh Token oluşturuldu", newRefreshToken.Token);
        await AddRefreshToken(newRefreshToken);

        return newRefreshToken;
    }
}