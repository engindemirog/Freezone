using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using AutoMapper;
using Freezone.Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.Revoke;

public class RevokeCommand : IRequest<RevokedResponse>
{
   public string Token { get; set; }
   public string IPAddress { get; set; }

   public class RevokeCommandHandler : IRequestHandler<RevokeCommand, RevokedResponse>
   {
        IRefreshTokenRepository _refreshTokenRepository;
        AuthBusinessRules _authBusinessRules;
        IAuthService _authService;
        private IMapper _mapper;

        public RevokeCommandHandler(IRefreshTokenRepository refreshTokenRepository, AuthBusinessRules authBusinessRules, IAuthService authService, IMapper mapper)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _authBusinessRules = authBusinessRules;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<RevokedResponse> Handle(RevokeCommand request, CancellationToken cancellationToken)
       {
            RefreshToken? refreshToken = await _refreshTokenRepository.GetAsync(rt => rt.Token == request.Token);
            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);
            await _authBusinessRules.RefreshTokenShouldBeActive(refreshToken!);

            await _authService.RevokeRefreshToken(refreshToken!, request.IPAddress, "Refresh Token manuel olarak geçersiz kılınmıştır.");

            RevokedResponse response = _mapper.Map<RevokedResponse>(refreshToken);
            return response;
       }
   }
}