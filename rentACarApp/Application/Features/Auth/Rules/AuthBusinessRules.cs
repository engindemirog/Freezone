using Application.Features.Auth.Constants;
using Application.Services.Repositories;
using Freezone.Core.Application.Rules;
using Freezone.Core.CrossCuttingConcerns.Exceptions;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.Hashing;

namespace Application.Features.Auth.Rules;

public class AuthBusinessRules : BaseBusinessRules
{
    private IUserRepository _userRepository;

    public AuthBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task UserEmailCannotBeDuplicatedWhenInserted(string email)
    {
        User? user = await _userRepository.GetAsync(u=>u.Email == email);
        if (user != null)
        {
            throw new BusinessException(AuthBusinessMessages.UserEmailAlreadyExists);
        }
    }

    public Task UserShouldBeExists(User? user)
    {
        if (user == null)
            throw new BusinessException(AuthBusinessMessages.UserNotFound);
        return Task.CompletedTask;
    }

    public Task UserPasswordShouldBeMatch(User user, string password)
    {
        bool isMatched = HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
        if (isMatched == false)
            throw new BusinessException(AuthBusinessMessages.UserPasswordNotMatch);
        return Task.CompletedTask;
    }
}