using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Freezone.Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.Refresh;

public class RefreshCommand : IRequest<RefreshedResponse>
{
    public string RefreshToken { get; set; }
    public string IpAddress { get; set; }

    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshedResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private IAuthService _authService;
        AuthBusinessRules _authBusinessRules;

        public RefreshCommandHandler(IAuthService authService, IRefreshTokenRepository refreshTokenRepository, AuthBusinessRules authBusinessRules)
        {
            _authService = authService;
            _refreshTokenRepository = refreshTokenRepository;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RefreshedResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository
                                                .GetAsync(rt => rt.Token == request.RefreshToken);
            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);
            
            if(refreshToken!.RevokedDate != null)
                await _authService.RevokeDescendantRefreshTokens(refreshToken, request.IpAddress, $"Geçersiz kılınmış Refresh Token kullanılmaya çalışıldı: {refreshToken.Token}");

            return new RefreshedResponse(); //TODO
        }
    }
}