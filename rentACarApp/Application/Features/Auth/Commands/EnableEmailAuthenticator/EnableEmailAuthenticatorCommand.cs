using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Freezone.Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand : IRequest
{
    public int UserId { get; set; }
    public string VerifyEmailUrl { get; set; }

    public class EnableEmailAuthenticatorCommandHandler : IRequestHandler<EnableEmailAuthenticatorCommand>
    {
        IUserRepository _userRepository;
        AuthBusinessRules _authBusinessRules;
        IUserEmailAuthenticatorRepository _userEmailAuthenticatorRepository;
        private IAuthService _authService;

        public EnableEmailAuthenticatorCommandHandler(IUserRepository userRepository, AuthBusinessRules authBusinessRules, IUserEmailAuthenticatorRepository userEmailAuthenticatorRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
            _userEmailAuthenticatorRepository = userEmailAuthenticatorRepository;
            _authService = authService;
        }

        public async Task<Unit> Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == request.UserId);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserShouldNotBeHasAuthenticator(user!);

            await _userEmailAuthenticatorRepository.DeleteAllNonVerifiedAsync(user!);
            UserEmailAuthenticator userEmailAuthenticator = await _authService.CreateEmailAuthenticator(user!);
            await _userEmailAuthenticatorRepository.AddAsync(userEmailAuthenticator);

            // TODO: Send Email with EmailService
            // $"{request.VerifyEmailUrl}?ActivationKey={userEmailAuthenticator.Key}"

            // TODO: Verify
            // TODO: Login'de Send Code
            // TODO: Login'de Verify Code

            return Unit.Value;
        }
    }
}