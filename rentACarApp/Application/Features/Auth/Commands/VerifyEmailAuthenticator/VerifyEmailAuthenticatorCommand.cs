using Application.Features.Auth.Rules;
using Application.Services.Repositories;
using Freezone.Core.Security.Authenticator;
using Freezone.Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.VerifyEmailAuthenticator;

public class VerifyEmailAuthenticatorCommand : IRequest
    // ISecuredOperation // Eğer kullanıcının giriş yapmış bir application'da (örn. react) ancak doğrulamasını istiyorsak ISecuredOperation ekleyebiliriz. UserId'i JWT'den alıcaz. Fakat bu eklenen ekstra bir önlem. Genel e-posta doğrulamalarında Activation Key'in gönderilmesi yeterli olur, ayrıcı login'e ihtiyaç duymaz.
{
    // public int UserId { get; set; }
    public string ActivationKey { get; set; }
    public string[] Roles => Array.Empty<string>();

    public class VerifyEmailAuthenticatorCommandHandler : IRequestHandler<VerifyEmailAuthenticatorCommand>
    {
        private readonly IUserEmailAuthenticatorRepository _userEmailAuthenticatorRepository;
        private readonly AuthBusinessRules _authBusinessRules;

        public VerifyEmailAuthenticatorCommandHandler(
            IUserEmailAuthenticatorRepository userEmailAuthenticatorRepository, AuthBusinessRules authBusinessRules)
        {
            _userEmailAuthenticatorRepository = userEmailAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
        }


        public async Task<Unit> Handle(VerifyEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            UserEmailAuthenticator? userEmailAuthenticator =
                await _userEmailAuthenticatorRepository.GetAsync(predicate: uea => //uea.UserId == request.UserId &&
                                                                     uea.Key == request.ActivationKey,
                                                                 include: uea => uea.Include(uea => uea.User));
            await _authBusinessRules.UserEmailAuthenticatorShouldBeExists(userEmailAuthenticator);

            await verifyEmailAuthenticator(
                userEmailAuthenticator!); // Uzun command'ler için bu şekilde refactor işlemi yapabiliriz. Bu command uzun değil ama yine de örnek olması için bu şekilde burayı bıraktım.

            return Unit.Value;
        }

        private async Task verifyEmailAuthenticator(UserEmailAuthenticator userEmailAuthenticator)
        {
            userEmailAuthenticator.IsVerified = true;
            userEmailAuthenticator.Key = null;
            userEmailAuthenticator.User.AuthenticatorType = AuthenticatorType.Email;
            await _userEmailAuthenticatorRepository.UpdateAsync(userEmailAuthenticator);
        }
    }
}