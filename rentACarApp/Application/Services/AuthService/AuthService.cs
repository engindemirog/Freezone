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
        var operationClaims = await _userOperationClaimRepository.Query().AsNoTracking()
                                                                 .Where(uoc => uoc.UserId == user.Id)
                                                                 .Include(uoc => uoc.OperationClaim)
                                                                 .Select(uoc => new OperationClaim
                                                                 {
                                                                     Id = uoc.OperationClaimId,
                                                                     Name = uoc.OperationClaim.Name
                                                                 }).ToListAsync();
        var accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }
}