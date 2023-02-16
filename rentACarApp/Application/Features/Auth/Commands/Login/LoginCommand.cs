using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Freezone.Core.Application.Dtos;
using Freezone.Core.Security.Authenticator;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoggedResponse>
{
    public UserForLoginDto UserForLoginDto { get; set; }
    public string IpAddress { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthService _authService;

        public LoginCommandHandler(IUserRepository userRepository, AuthBusinessRules authBusinessRules,
                                   IAuthService authService)
        {
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
            _authService = authService;
        }

        public async Task<LoggedResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(u => u.Email == request.UserForLoginDto.Email);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserPasswordShouldBeMatch(user: user!, request.UserForLoginDto.Password);

            LoggedResponse response = new();
            if (user!.AuthenticatorType is not AuthenticatorType.None)
            {
                if (request.UserForLoginDto.AuthenticatorCode is null)
                {
                    await _authService.SendAuthenticatorCode(user);
                    response.RequiredAuthenticatorType = user.AuthenticatorType;
                    return response;
                }
                else
                {
                    await _authService.VerifyAuthenticatorCode(user, request.UserForLoginDto.AuthenticatorCode);
                }
            } 
            
            AccessToken createdAccessToken = await _authService.CreateAccessToken(user!);

            await _authService.DeleteOldActiveRefreshTokens(user!);
            RefreshToken refreshToken = await _authService.CreateRefreshToken(user: user!, request.IpAddress);
            await _authService.AddRefreshToken(refreshToken);

            response.AccessToken = createdAccessToken;
            response.RefreshToken = refreshToken;
            return response;
        }
    }
}