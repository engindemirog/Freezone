using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.Refresh;

public class RefreshCommand : IRequest<RefreshedResponse>
{
    public string RefreshToken { get; set; }
    public string IpAddress { get; set; }

    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshedResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private IAuthService _authService;
        IUserRepository _userRepository;
        AuthBusinessRules _authBusinessRules;

        public RefreshCommandHandler(IAuthService authService, IRefreshTokenRepository refreshTokenRepository, AuthBusinessRules authBusinessRules, IUserRepository userRepository)
        {
            _authService = authService;
            _refreshTokenRepository = refreshTokenRepository;
            _authBusinessRules = authBusinessRules;
            _userRepository = userRepository;
        }

        public async Task<RefreshedResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository
                                                .GetAsync(rt => rt.Token == request.RefreshToken,
                                                    include: rt => rt.Include(r => r.User));
            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);
            
            if(refreshToken!.RevokedDate != null)
                await _authService.RevokeDescendantRefreshTokens(refreshToken, request.IpAddress, $"Geçersiz kılınmış Refresh Token kullanılmaya çalışıldı: {refreshToken.Token}");
            await _authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(refreshToken.User);
            RefreshToken createdRefreshToken = await _authService.RotateRefreshToken(refreshToken.User, refreshToken, request.IpAddress);

            RefreshedResponse response = new()
            {
                AccessToken = createdAccessToken,
                RefreshToken = createdRefreshToken
            };
           return response;
        }
    }
}