using Application.Services.Repositories;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.AuthService;

public class AuthService : IAuthService
{
    private IUserOperationClaimRepository _userOperationClaimRepository;
    private ITokenHelper _tokenHelper;

    public AuthService(IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
        _tokenHelper = tokenHelper;
    }

    public async Task<AccessToken> CreateAccessToken(User user)
    {
        var operationClaims = await _userOperationClaimRepository.GetOperationClaimsByUserIdAsync(user.Id);
        var accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }
}