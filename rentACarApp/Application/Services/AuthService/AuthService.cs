using Application.Services.Repositories;
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
        ICollection<RefreshToken> oldActiveRefreshTokens = await _refreshTokenRepository
                                                                   .GetAllOldActiveRefreshTokensAsync(
                                                                       user, _tokenHelper.RefreshTokenTTLOption);
        await _refreshTokenRepository.DeleteRangeAsync(oldActiveRefreshTokens.ToList());
    }

    public Task RevokeRefreshToken(RefreshToken token, string ipAddress, string reason) => throw new NotImplementedException();

    public Task RevokeDescendantRefreshTokens(RefreshToken token, string ipAddress, string reason) => throw new NotImplementedException();

    public Task<RefreshToken> RotateRefreshToken(RefreshToken token, string ipAddress) => throw new NotImplementedException();
}