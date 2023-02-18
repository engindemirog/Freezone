using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Freezone.Core.Application.Pipelines.Authorization;
using Freezone.Core.Security.Authenticator;
using Freezone.Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.VerifyOtpAuthenticator;

public class VerifyOtpAuthenticatorCommand : IRequest, ISecuredOperation
{
    public int UserId { get; set; }
    public string OtpCode { get; set; }
    
    public string[] Roles => Array.Empty<string>();

    public class VerifyOtpAuthenticatorCommandHandler : IRequestHandler<VerifyOtpAuthenticatorCommand>
    {
        private IUserOtpAuthenticatorRepository _userOtpAuthenticatorRepository;
        private AuthBusinessRules _authBusinessRules;
        private IAuthService _authService;

        public VerifyOtpAuthenticatorCommandHandler(IUserOtpAuthenticatorRepository userOtpAuthenticatorRepository, AuthBusinessRules authBusinessRules, IAuthService authService)
        {
            _userOtpAuthenticatorRepository = userOtpAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
            _authService = authService;
        }

        public async Task<Unit> Handle(VerifyOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            UserOtpAuthenticator? userOtpAuthenticator =
                await _userOtpAuthenticatorRepository.GetAsync(uoa => uoa.UserId == request.UserId,
                                                               include: uoa => uoa.Include(uoa => uoa.User));
            await _authBusinessRules.UserOtpAuthenticatorShouldBeExists(userOtpAuthenticator);

            userOtpAuthenticator!.User.AuthenticatorType = AuthenticatorType.Otp;
            await _authService.VerifyAuthenticatorCode(userOtpAuthenticator.User, request.OtpCode);
            userOtpAuthenticator.IsVerified = true;
            await _userOtpAuthenticatorRepository.UpdateAsync(userOtpAuthenticator);

            return Unit.Value;
        }
    }
}