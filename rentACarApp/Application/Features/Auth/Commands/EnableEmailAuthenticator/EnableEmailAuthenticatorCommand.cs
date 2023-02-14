using System.Web;
using Application.Features.Auth.Constants;
using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Freezone.Core.Application.Pipelines.Authorization;
using Freezone.Core.Mailing;
using Freezone.Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand : IRequest, ISecuredOperation
{
    public int UserId { get; set; }
    public string VerifyEmailUrl { get; set; }
    public string[] Roles => Array.Empty<string>();

    public class EnableEmailAuthenticatorCommandHandler : IRequestHandler<EnableEmailAuthenticatorCommand>
    {
        IUserRepository _userRepository;
        AuthBusinessRules _authBusinessRules;
        IUserEmailAuthenticatorRepository _userEmailAuthenticatorRepository;
        private IAuthService _authService;
        private IMailService _mailService;

        public EnableEmailAuthenticatorCommandHandler(IUserRepository userRepository, AuthBusinessRules authBusinessRules, IUserEmailAuthenticatorRepository userEmailAuthenticatorRepository, IAuthService authService, IMailService mailService)
        {
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
            _userEmailAuthenticatorRepository = userEmailAuthenticatorRepository;
            _authService = authService;
            _mailService = mailService;
        }

        public async Task<Unit> Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == request.UserId);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserShouldNotBeHasAuthenticator(user!);

            await _userEmailAuthenticatorRepository.DeleteAllNonVerifiedAsync(user!);
            UserEmailAuthenticator userEmailAuthenticator = await _authService.CreateEmailAuthenticator(user!);
            await _userEmailAuthenticatorRepository.AddAsync(userEmailAuthenticator);

            Mail mailData = new()
            {
                ToEmail = user!.Email,
                ToFullName = $"{user.FirstName} {user.LastName}",
                Subject = AuthBusinessMessages.VerifyEmail,
                TextBody = $"{AuthBusinessMessages.ClickOnBelowLinkToVerifyEmail}\n" +
                           $"{request.VerifyEmailUrl}?activationKey={HttpUtility.UrlEncode(userEmailAuthenticator.Key)}"
            };
            await _mailService.SendAsync(mailData);

            return Unit.Value;
        }
    }

}