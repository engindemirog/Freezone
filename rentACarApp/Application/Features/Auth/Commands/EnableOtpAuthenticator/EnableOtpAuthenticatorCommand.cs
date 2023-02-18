using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Freezone.Core.Application.Pipelines.Authorization;
using Freezone.Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand : IRequest<EnabledOtpAuthenticatorResponse>, ISecuredOperation
{
    public int UserId { get; set; }
    public string SecretKeyLabel { get; set; }
    public string SecretKeyIssuer { get; set; }

    public string[] Roles => Array.Empty<string>();

    public class EnableOtpAuthenticatorCommandHandler : IRequestHandler<EnableOtpAuthenticatorCommand,
        EnabledOtpAuthenticatorResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthBusinessRules _authBusinessRules;
        private IAuthService _authService;
        IUserOtpAuthenticatorRepository _userOtpAuthenticatorRepository;

        public EnableOtpAuthenticatorCommandHandler(IUserRepository userRepository, AuthBusinessRules authBusinessRules, IAuthService authService, IUserOtpAuthenticatorRepository userOtpAuthenticatorRepository)
        {
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
            _authService = authService;
            _userOtpAuthenticatorRepository = userOtpAuthenticatorRepository;
        }

        public async Task<EnabledOtpAuthenticatorResponse> Handle(EnableOtpAuthenticatorCommand request,
                                                                  CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == request.UserId);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserShouldNotBeHasAuthenticator(user);

            // TODO: önceden oluşturulmuş fakat verify edilmemiş UserOtpAuthenticator varsa silinmelidir.
            UserOtpAuthenticator createdUserOtpAuthenticator = await _authService.CreateOtpAuthenticator(user);
            await _userOtpAuthenticatorRepository.AddAsync(createdUserOtpAuthenticator);

            string base32SecretKey =
                await _authService.ConvertOtpSecretKeyToString(createdUserOtpAuthenticator.SecretKey);
            EnabledOtpAuthenticatorResponse response = new()
            {
                SecretKey = base32SecretKey,
                SecketKeyUrl = $"otpauth://totp/{request.SecretKeyLabel}?secret={base32SecretKey}&issuer={request.SecretKeyIssuer}"
            };
            return response;
        }
    }

}